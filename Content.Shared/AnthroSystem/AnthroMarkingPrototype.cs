using System.Collections.Generic;
using Content.Shared.Body.Part;
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

        [DataField("bodyPart")]
        public HumanoidVisualLayers BodyPart { get; } = default!;

        [DataField("sprite", required: true)]
        public SpriteSpecifier Sprite { get; } = default!;

        public AnthroMarking AsMarking()
        {
            return new AnthroMarking(ID, null);
        }

        void ISerializationHooks.AfterDeserialization()
        {
            Name = Loc.GetString($"marking-{ID}");
        }
    }
}
