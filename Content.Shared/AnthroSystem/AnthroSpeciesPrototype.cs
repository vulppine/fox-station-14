using System.Collections.Generic;
using Content.Shared.CharacterAppearance;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Utility;

namespace Content.Shared.AnthroSystem
{
    [Prototype("anthroSpecies")]
    public sealed class AnthroSpeciesPrototype : IPrototype
    {
        [DataField("id", required: true)]
        public string ID { get; } = "species";

        // Easily map visual layers to sprites in one go.
        [DataField("speciesSprite", required: true)]
        public Dictionary<HumanoidVisualLayers, SpriteSpecifier?> SpeciesParts { get; } = default!;
    }
}
