using System;
using System.Collections.Generic;
using System.Linq;
using Content.Shared.AnthroSystem;
using Content.Shared.Body.Components;
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
        private IReadOnlyList<AnthroMarking>? _lastMarkingSet;
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

        // This should *probably* be in the main body.
        // Maybe if this is ever done upstream.
        protected override void Initialize()
        {
            base.Initialize();

            InitAnthroSystem();
        }

        private void InitAnthroSystem()
        {
            foreach (HumanoidVisualLayers layer in actualBodyParts)
            {
                Logger.DebugS("AnthroSystem", $"Activating marking tracking for {layer}");
                _activeMarkings.Add(layer, new List<AnthroMarking>());
            }
        }

        private void ToggleMarkingVisibility(SpriteComponent body, HumanoidVisualLayers layer, bool toggle)
        {
            if (_activeMarkings.TryGetValue(layer, out List<AnthroMarking>? layerMarkings))
                foreach (AnthroMarking marking in layerMarkings)
                    body.LayerSetVisible(marking.MarkingId, toggle);
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


                // Top -> Bottom ordering
                foreach (var marking in Appearance.Markings.Reverse())
                {
                    if (!_markingManager.IsValidMarking(marking, out AnthroMarkingPrototype? markingPrototype))
                    {
                        Logger.DebugS("AnthroSystem", $"Invalid marking {marking.MarkingId}");
                        continue;
                    }


                    if (!partSprite.LayerMapTryGet(markingPrototype.BodyPart, out int targetLayer))
                    {
                        Logger.DebugS("AnthroSystem", "Could not get the target layer");
                        continue;
                    }

                    Logger.DebugS("AnthroSystem", $"Adding {markingPrototype.Sprites.Count()} markings from {markingPrototype.ID} to layer {targetLayer}");

                    for (int i = 0; i < markingPrototype.Sprites.Count(); i++)
                    {
                        string layerId = markingPrototype.ID + markingPrototype.MarkingPartNames[i];

                        if (partSprite.LayerMapTryGet(layerId, out var existingLayer))
                        {
                            Logger.DebugS("AnthroSystem", $"Deduplicating {markingPrototype.MarkingPartNames[i]} now from {existingLayer}");
                            partSprite.RemoveLayer(existingLayer);
                            partSprite.LayerMapRemove(marking.MarkingId);
                        }
                        Logger.DebugS("AnthroSystem", $"Adding part {markingPrototype.MarkingPartNames[i]} now to {targetLayer + i + 1}");

                        int layer = partSprite.AddLayer(markingPrototype.Sprites[i], targetLayer + i + 1);
                        partSprite.LayerMapSet(layerId, layer);
                        partSprite.LayerSetColor(layerId, marking.MarkingColors[i]);
                    }

                    Logger.DebugS("AnthroSystem", $"Marking added: {markingPrototype.ID}");
                    // _activeMarkings[markingPrototype.BodyPart].Add(marking);
                }
            }
        }

    }
}
