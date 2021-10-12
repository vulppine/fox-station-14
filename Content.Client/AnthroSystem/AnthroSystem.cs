using System.Collections.Generic;
using System.Linq;
using Content.Shared.AnthroSystem;
using Content.Shared.CharacterAppearance;
using Content.Shared.Preferences;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Utility;

namespace Content.Client.AnthroSystem
{
    public class AnthroEntitySystem : EntitySystem
    {
        [Dependency] private readonly AnthroMarkingManager _markingManager = default!;
        [Dependency] private readonly AnthroSpeciesManager _speciesManager = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<AnthroComponent, ComponentInit>(OnAnthroSystemInit);
        }

        static HumanoidVisualLayers[] actualBodyParts =
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
        public static string DefaultBase = "human";


        private void OnAnthroSystemInit(EntityUid uid, AnthroComponent component, ComponentInit __)
        {
            foreach (HumanoidVisualLayers layer in actualBodyParts)
            {
                Logger.DebugS("AnthroSystem", $"Activating marking tracking for {layer}");
                component.ActiveMarkings.Add(layer, new List<AnthroMarking>());
            }

        }

        // we use HumanoidCharacterProfile because it already exists -
        // we just grab the inner Appearance from it since we've tied it in
        // severely to that
        //
        // Probably can untie it, though
        //
        // TODO: Untie Appearance.Markings, Appearance.SpeciesBase from
        // HumanoidCharacterAppearance, inline with the whole SQL model
        // thing as well probably
        public void UpdateMarkings(EntityUid uid, HumanoidCharacterAppearance appearance)
        {
            var owner = EntityManager.GetEntity(uid);
            if(!owner.TryGetComponent(out AnthroComponent? anthroSystem)
                || !owner.TryGetComponent(out SpriteComponent? sprite)) return;

            if (appearance.SpeciesBase != anthroSystem.LastBase
                    && _speciesManager.SpeciesHasSprites(appearance.SpeciesBase,
                    out IReadOnlyCollection<KeyValuePair<HumanoidVisualLayers, SpriteSpecifier?>>? speciesParts))
            {
                Logger.DebugS("AnthroSystem", "Rerendering body sprites due to species difference.");
                anthroSystem.LastBase = appearance.SpeciesBase;
                foreach (var (layer, speciesSprite) in speciesParts)
                    if (speciesSprite is not null) sprite.LayerSetSprite(layer, speciesSprite);
            }

            Logger.DebugS("AnthroSystem", "Recoloring body now.");
            foreach (var part in actualBodyParts)
            {
                if (!sprite.LayerMapTryGet(part, out int targetLayer))
                {
                    Logger.DebugS("AnthroSystem", $"Could not get layer {part}");
                    continue;
                }

                sprite.LayerSetColor(targetLayer, appearance.SkinColor);
            }

            Logger.DebugS("AnthroSystem", "Rendering markings now.");
            Logger.DebugS("AnthroSystem", $"Marking count: {appearance.Markings.Count}");


            // Top -> Bottom ordering
            foreach (var marking in appearance.Markings.Reverse())
            {
                if (!_markingManager.IsValidMarking(marking, out AnthroMarkingPrototype? markingPrototype))
                {
                    Logger.DebugS("AnthroSystem", $"Invalid marking {marking.MarkingId}");
                    continue;
                }


                if (!sprite.LayerMapTryGet(markingPrototype.BodyPart, out int targetLayer))
                {
                    Logger.DebugS("AnthroSystem", "Could not get the target layer");
                    continue;
                }

                Logger.DebugS("AnthroSystem", $"Adding {markingPrototype.Sprites.Count()} markings from {markingPrototype.ID} to layer {targetLayer}");

                for (int i = 0; i < markingPrototype.Sprites.Count(); i++)
                {
                    string layerId = markingPrototype.ID + markingPrototype.MarkingPartNames[i];

                    if (sprite.LayerMapTryGet(layerId, out var existingLayer))
                    {
                        Logger.DebugS("AnthroSystem", $"Deduplicating {markingPrototype.MarkingPartNames[i]} now from {existingLayer}");
                        sprite.RemoveLayer(existingLayer);
                        sprite.LayerMapRemove(marking.MarkingId);
                    }
                    Logger.DebugS("AnthroSystem", $"Adding part {markingPrototype.MarkingPartNames[i]} now to {targetLayer + i + 1}");

                    int layer = sprite.AddLayer(markingPrototype.Sprites[i], targetLayer + i + 1);
                    sprite.LayerMapSet(layerId, layer);
                    sprite.LayerSetColor(layerId, marking.MarkingColors[i]);
                }

                Logger.DebugS("AnthroSystem", $"Marking added: {markingPrototype.ID}");
                // _activeMarkings[markingPrototype.BodyPart].Add(marking);
            }
        }

        public void UpdateMarkings(EntityUid uid, HumanoidCharacterProfile profile)
        {
            UpdateMarkings(uid, profile.Appearance);
        }
    }
}
