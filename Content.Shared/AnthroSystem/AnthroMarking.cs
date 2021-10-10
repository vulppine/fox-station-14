using System;
using Content.Shared.Body.Part;
using Robust.Shared.Maths;
using Robust.Shared.Serialization;

namespace Content.Shared.AnthroSystem
{
    [Serializable, NetSerializable]
    public class AnthroMarking : IEquatable<AnthroMarking>, IComparable<AnthroMarking>, IComparable<string>
    {
        public AnthroMarking(string markingId,
            Color? markingColor)
        {
            MarkingId = markingId;
            MarkingColor = markingColor ?? Color.Black;
        }

        public string MarkingId { get; } = default!;
        public Color MarkingColor { get; set; } = Color.Black;

        public int CompareTo(AnthroMarking? marking)
        {
            if (marking == null) return 1;
            else return this.MarkingId.CompareTo(marking.MarkingId);
        }

        public int CompareTo(string? markingId)
        {
            if (markingId == null) return 1;
            return this.MarkingId.CompareTo(markingId);
        }

        public bool Equals(AnthroMarking? other)
        {
            if (other == null) return false;
            return (this.MarkingId.Equals(other.MarkingId));
        }

        // This is ABSURDLY messy,
        // but only because I want to **avoid** putting
        // a dynamic flat file prototype struct into SQL
        // that would end up translating to a many-many
        // while also attempting to keep layer rank
        // it's just really messy.
        new public string ToString()
        {
            // reserved character
            string sanitizedName = this.MarkingId.Replace('%', '_');
            return $"{sanitizedName}%{this.MarkingColor.ToHex()}";
        }

        public static AnthroMarking? ParseFromDbString(string input)
        {
            if (input.Length == 0) return null;
            var split = input.Split('%');
            if (split.Length != 2) return null;

            return new AnthroMarking(split[0], Color.FromHex(split[1]));
        }
    }
}
