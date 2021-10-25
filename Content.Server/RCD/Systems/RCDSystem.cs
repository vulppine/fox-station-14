using Content.Server.DoAfter;
using Content.Server.RCD.Components;
using Content.Shared.Coordinates;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Helpers;
using Content.Shared.Maps;
using Content.Shared.Popups;
using Content.Shared.Tag;
using Robust.Shared.Audio;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Player;
using System;
using System.Threading;

namespace Content.Server.RCD.Systems
{
    public class RCDSystem : EntitySystem
    {
        [Dependency] private readonly ITileDefinitionManager _tileDefinitionManager = default!;
        [Dependency] private readonly IMapManager _mapManager = default!;

        [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;

        private readonly RcdMode[] _modes = (RcdMode[]) Enum.GetValues(typeof(RcdMode));

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<RCDComponent, MapInitEvent>(OnMapInit);
            SubscribeLocalEvent<RCDComponent, ExaminedEvent>(OnExamine);
            SubscribeLocalEvent<RCDComponent, UseInHandEvent>(OnUseInHand);
            SubscribeLocalEvent<RCDComponent, AfterInteractEvent>(OnAfterInteract);
        }

        private void OnMapInit(EntityUid uid, RCDComponent component, MapInitEvent args)
        {
            component.CurrentAmmo = component.StartingAmmo;
        }

        private void OnExamine(EntityUid uid, RCDComponent component, ExaminedEvent args)
        {
            var msg = Loc.GetString("rcd-component-examine-detail-count",
                ("mode", component.Mode), ("ammoCount", component.CurrentAmmo));
            args.PushMarkup(msg);
        }

        private void OnUseInHand(EntityUid uid, RCDComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            NextMode(uid, component, args.User);
            args.Handled = true;
        }

        private async void OnAfterInteract(EntityUid uid, RCDComponent rcd, AfterInteractEvent args)
        {
            if (args.Handled)
                return;

            // FIXME: Make this work properly. Right now it relies on the click location being on a grid, which is bad.
            if (!args.ClickLocation.IsValid(EntityManager))
                return;

            var gridID = args.ClickLocation.GetGridId(EntityManager);
            if (!gridID.IsValid())
                return;

            var mapGrid = _mapManager.GetGrid(gridID);
            var tile = mapGrid.GetTileRef(args.ClickLocation);
            var snapPos = mapGrid.TileIndicesFor(args.ClickLocation);

            //No changing mode mid-RCD
            var startingMode = rcd.Mode;

            //Using an RCD isn't instantaneous
            var cancelToken = new CancellationTokenSource();
            var doAfterEventArgs = new DoAfterEventArgs(args.User, rcd.Delay, cancelToken.Token, args.Target)
            {
                BreakOnDamage = true,
                BreakOnStun = true,
                NeedHand = true,
                ExtraCheck = () => IsRCDStillValid(rcd, args, mapGrid, tile, snapPos, startingMode) //All of the sanity checks are here
            };

            var result = await _doAfterSystem.WaitDoAfter(doAfterEventArgs);
            if (result == DoAfterStatus.Cancelled)
            {
                args.Handled = true;
                return;
            }

            switch (rcd.Mode)
            {
                //Floor mode just needs the tile to be a space tile (subFloor)
                case RcdMode.Floors:
                    mapGrid.SetTile(args.ClickLocation, new Tile(_tileDefinitionManager["floor_steel"].TileId));
                    break;
                //We don't want to place a space tile on something that's already a space tile. Let's do the inverse of the last check.
                case RcdMode.Deconstruct:
                    if (!tile.IsBlockedTurf(true)) //Delete the turf
                    {
                        mapGrid.SetTile(snapPos, Tile.Empty);
                    }
                    else //Delete what the user targeted
                    {
                        args.Target?.Delete();
                    }
                    break;
                //Walls are a special behaviour, and require us to build a new object with a transform rather than setting a grid tile,
                // thus we early return to avoid the tile set code.
                case RcdMode.Walls:
                    var ent = EntityManager.SpawnEntity("WallSolid", mapGrid.GridTileToLocal(snapPos));
                    ent.Transform.LocalRotation = Angle.Zero; // Walls always need to point south.
                    break;
                case RcdMode.Airlock:
                    var airlock = EntityManager.SpawnEntity("Airlock", mapGrid.GridTileToLocal(snapPos));
                    airlock.Transform.LocalRotation = rcd.Owner.Transform.LocalRotation; //Now apply icon smoothing.
                    break;
                default:
                    args.Handled = true;
                    return; //I don't know why this would happen, but sure I guess. Get out of here invalid state!
            }

            SoundSystem.Play(Filter.Pvs(uid), rcd.SuccessSound.GetSound(), rcd.Owner);
            rcd.CurrentAmmo--;
            args.Handled = true;
        }

        private bool IsRCDStillValid(RCDComponent rcd, AfterInteractEvent eventArgs, IMapGrid mapGrid, TileRef tile, Vector2i snapPos, RcdMode startingMode)
        {
            //Less expensive checks first. Failing those ones, we need to check that the tile isn't obstructed.
            if (rcd.CurrentAmmo <= 0)
            {
                rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-no-ammo-message"));
                return false;
            }

            if (rcd.Mode != startingMode)
            {
                return false;
            }

            var coordinates = mapGrid.ToCoordinates(tile.GridIndices);
            if (coordinates == EntityCoordinates.Invalid || !eventArgs.InRangeUnobstructed(ignoreInsideBlocker: true, popup: true))
            {
                return false;
            }

            switch (rcd.Mode)
            {
                //Floor mode just needs the tile to be a space tile (subFloor)
                case RcdMode.Floors:
                    if (!tile.Tile.IsEmpty)
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-cannot-build-floor-tile-not-empty-message"));
                        return false;
                    }

                    return true;
                //We don't want to place a space tile on something that's already a space tile. Let's do the inverse of the last check.
                case RcdMode.Deconstruct:
                    if (tile.Tile.IsEmpty)
                    {
                        return false;
                    }

                    //They tried to decon a turf but the turf is blocked
                    if (eventArgs.Target == null && tile.IsBlockedTurf(true))
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-tile-obstructed-message"));
                        return false;
                    }
                    //They tried to decon a non-turf but it's not in the whitelist
                    if (eventArgs.Target != null && !eventArgs.Target.HasTag("RCDDeconstructWhitelist"))
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-deconstruct-target-not-on-whitelist-message"));
                        return false;
                    }

                    return true;
                //Walls are a special behaviour, and require us to build a new object with a transform rather than setting a grid tile, thus we early return to avoid the tile set code.
                case RcdMode.Walls:
                    if (tile.Tile.IsEmpty)
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-cannot-build-wall-tile-not-empty-message"));
                        return false;
                    }

                    if (tile.IsBlockedTurf(true))
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-tile-obstructed-message"));
                        return false;
                    }
                    return true;
                case RcdMode.Airlock:
                    if (tile.Tile.IsEmpty)
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-cannot-build-airlock-tile-not-empty-message"));
                        return false;
                    }
                    if (tile.IsBlockedTurf(true))
                    {
                        rcd.Owner.PopupMessage(eventArgs.User, Loc.GetString("rcd-component-tile-obstructed-message"));
                        return false;
                    }
                    return true;
                default:
                    return false; //I don't know why this would happen, but sure I guess. Get out of here invalid state!
            }
        }

        private void NextMode(EntityUid uid, RCDComponent rcd, IEntity? user)
        {
            SoundSystem.Play(Filter.Pvs(uid), rcd.SwapModeSound.GetSound(), uid);

            var mode = (int) rcd.Mode; //Firstly, cast our RCDmode mode to an int (enums are backed by ints anyway by default)
            mode = (++mode) % _modes.Length; //Then, do a rollover on the value so it doesnt hit an invalid state
            rcd.Mode = (RcdMode) mode; //Finally, cast the newly acquired int mode to an RCDmode so we can use it.

            if (user != null)
            {
                var msg = Loc.GetString("rcd-component-change-mode", ("mode", rcd.Mode.ToString()));
                rcd.Owner.PopupMessage(user, msg); //Prints an overhead message above the RCD
            }
        }
    }
}
