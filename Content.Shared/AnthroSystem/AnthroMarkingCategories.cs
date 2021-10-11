using System;
using Robust.Shared.Serialization;

namespace Content.Shared.AnthroSystem
{
    [Serializable, NetSerializable]
    public enum AnthroMarkingCategories
    {
        Head,
        Chest,
        Arms,
        Legs,
        Ears,
        Tail,
        Overlay
    }
}
