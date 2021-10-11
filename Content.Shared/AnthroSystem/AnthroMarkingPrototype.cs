using System.Collections.Generic;
using Content.Shared.CharacterAppearance;
using Robust.Shared.Localization;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Utility;

namespace Content.Shared.AnthroSystem
{
    [Prototype("anthroMarking")]
    public class AnthroMarkingPrototype : IPrototype, ISerializationHooks
    {
        [DataField("id", required: true)]
        public string ID { get; } = "uwu";

        public string Name { get; private set; } = default!;

        [DataField("bodyPart", required: true)]
        public HumanoidVisualLayers BodyPart { get; } = default!;

        [DataField("markingLayerNames", required: true)]
        public List<string> MarkingPartNames { get; } = default!;

        [DataField("markingCategory", required: true)]
        public AnthroMarkingCategories MarkingCategory { get; } = default!;

        [DataField("sprites", required: true)]
        public List<SpriteSpecifier> Sprites { get; private set; } = default!;

        public AnthroMarking AsMarking()
        {
            return new AnthroMarking(ID, Sprites.Count);
        }

        void ISerializationHooks.AfterDeserialization()
        {
            Name = Loc.GetString($"marking-{ID}");
        }
    }
}
