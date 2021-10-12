using System.Collections.Generic;
using Content.Shared.CharacterAppearance;
using Robust.Shared.GameObjects;

namespace Content.Shared.AnthroSystem
{
    [RegisterComponent]
    public class AnthroComponent : Component
    {
        public override string Name => "AnthroSystem";

        public List<AnthroMarking> CurrentMarkingSet = new();
        public List<AnthroMarking> LastMarkingSet = new();
        public string LastBase = "human";
        public Dictionary<HumanoidVisualLayers, List<AnthroMarking>> ActiveMarkings = new();
    }
}
