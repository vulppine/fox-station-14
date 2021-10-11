using System;
using System.Collections.Generic;
using System.Linq;
using Content.Shared.AnthroSystem;
using Content.Shared.Body.Components;
using Content.Shared.Body.Part;
using Content.Shared.CharacterAppearance;
using Robust.Client.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Utility;

namespace Content.Client.CharacterAppearance
{
    public sealed partial class HumanoidAppearanceComponent
    {
        [Dependency] private readonly AnthroMarkingManager _markingManager = default!;
        [Dependency] private readonly AnthroSpeciesManager _speciesManager = default!;

        private Dictionary<HumanoidVisualLayers, List<AnthroMarking>> _activeMarkings = new();
        // the default body is a human body, so they always spawn with human parts
        private string _lastBase = AnthroSpeciesManager.DefaultBase;

        public static HumanoidVisualLayers[] actualBodyParts =
        {
            HumanoidVisualLayers.Chest,
            HumanoidVisualLayers.Head,
            HumanoidVisualLayers.RArm,
            HumanoidVisualLayers.LArm,
            HumanoidVisualLayers.RHand,
            HumanoidVisualLayers.LHand,
            HumanoidVisualLayers.RLeg,
            HumanoidVisualLayers.LLeg,
            HumanoidVisualLayers.RFoot,
            HumanoidVisualLayers.LFoot
        };


        private void InitAnthroSystem()
        {
            foreach (HumanoidVisualLayers layer in actualBodyParts)
            {
                _activeMarkings.Add(layer, new List<AnthroMarking>());
            }
        }

        private void UpdateAnthroSystem()
        {
            if (Owner.TryGetComponent(out SharedBodyComponent? body)
                    && Owner.TryGetComponent(out SpriteComponent? partSprite))
            {
                // This is pretty messy. It would be nicer to have a facility to
                // instead show a different species when selected based on
                // prototype bodies, rather than prototypes with
                // a dictionary of replacement parts.
                //
                // That would be nice as an upstream PR. :fox:
                if (Appearance.SpeciesBase != _lastBase && _speciesManager.SpeciesHasSprites(Appearance.SpeciesBase,
                        out IReadOnlyCollection<KeyValuePair<HumanoidVisualLayers, SpriteSpecifier?>>? speciesParts))
                {
                    Logger.DebugS("AnthroSystem", "Rerendering body sprites due to species difference.");
                    _lastBase = Appearance.SpeciesBase;
                    foreach (var (layer, sprite) in speciesParts)
                        if (sprite is not null) partSprite.LayerSetSprite(layer, sprite);
                }


                Logger.DebugS("AnthroSystem", "Recoloring body now.");
                foreach (var part in actualBodyParts)
                {
                    if (!partSprite.LayerMapTryGet(part, out int targetLayer))
                    {
                        Logger.DebugS("AnthroSystem", $"Could not get layer {part}");
                        continue;
                    }

                    partSprite.LayerSetColor(targetLayer, Appearance.SkinColor);
                }

                Logger.DebugS("AnthroSystem", "Rendering markings now.");
                Logger.DebugS("AnthroSystem", $"Marking count: {Appearance.Markings.Count}");

                foreach (var marking in Appearance.Markings)
                {
                    // there is NO EASY WAY to do this
                    // so we just iterate over every single
                    // part, and attempt to remove any layers
                    // that match
                    if (!partSprite.LayerMapTryGet(marking.MarkingId, out var layer)) continue;
                    partSprite.RemoveLayer(layer);
                    partSprite.LayerMapRemove(marking.MarkingId);
                }

                // Top -> Bottom ordering
                foreach (var marking in Appearance.Markings.Reverse())
                {
                    if (!_markingManager.IsValidMarking(marking.MarkingId, out AnthroMarkingPrototype? markingPrototype))
                    {
                        Logger.DebugS("AnthroSystem", $"Invalid marking {marking.MarkingId}");
                        continue;
                    }

                    if (!partSprite.LayerMapTryGet(markingPrototype.BodyPart, out int targetLayer))
                    {
                        Logger.DebugS("AnthroSystem", "Could not get the target layer");
                    }

                    Logger.DebugS("AnthroSystem", $"Adding marking {marking.MarkingId} to layer {targetLayer}");
                    int layer = partSprite.AddLayer(markingPrototype.Sprite, targetLayer + 1);
                    partSprite.LayerMapSet(markingPrototype.ID, layer);
                    Logger.DebugS("AnthroSystem", $"Marking added {marking.MarkingId} to layer {layer}");
                    partSprite.LayerSetColor(markingPrototype.ID, marking.MarkingColor);
                    // _activeMarkings[markingPrototype.BodyPart].Add(marking);
                }
            }
        }

    }
}
