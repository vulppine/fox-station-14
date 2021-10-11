using System;
using System.Collections.Generic;
using System.Linq;
using Content.Shared.AnthroSystem;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Random;
using Robust.Shared.Serialization;

namespace Content.Shared.CharacterAppearance
{
    // Major changes here to mimic how HumanoidCharacterProfile
    // deals with collections.
    [Serializable, NetSerializable]
    public class HumanoidCharacterAppearance : ICharacterAppearance
    {
        private readonly List<AnthroMarking> _markings;

        private HumanoidCharacterAppearance(string hairStyleId,
            Color hairColor,
            string facialHairStyleId,
            Color facialHairColor,
            Color eyeColor,
            Color skinColor,
            List<AnthroMarking> markings,
            string speciesBase)
        {
            HairStyleId = hairStyleId;
            HairColor = ClampColor(hairColor);
            FacialHairStyleId = facialHairStyleId;
            FacialHairColor = ClampColor(facialHairColor);
            EyeColor = ClampColor(eyeColor);
            SkinColor = ClampColor(skinColor);
            _markings = markings;
            SpeciesBase = speciesBase;
        }

        public HumanoidCharacterAppearance(string hairStyleId,
            Color hairColor,
            string facialHairStyleId,
            Color facialHairColor,
            Color eyeColor,
            Color skinColor,
            IReadOnlyList<AnthroMarking> markings,
            string speciesBase)
            : this(hairStyleId,
                hairColor,
                facialHairStyleId,
                facialHairColor,
                eyeColor,
                skinColor,
                new List<AnthroMarking>(markings),
                speciesBase)
        {
        }


        public string HairStyleId { get; }
        public Color HairColor { get; }
        public string FacialHairStyleId { get; }
        public Color FacialHairColor { get; }
        public Color EyeColor { get; }
        public Color SkinColor { get; }
        // ANTHROSYSTEM MODIFICATION
        public IReadOnlyList<AnthroMarking> Markings => _markings;
        public string SpeciesBase { get; }
        // ANTHROSYSTEM MODIFICATION

        public HumanoidCharacterAppearance WithHairStyleName(string newName)
        {
            return new(newName, HairColor, FacialHairStyleId, FacialHairColor, EyeColor, SkinColor, Markings, SpeciesBase);
        }

        public HumanoidCharacterAppearance WithHairColor(Color newColor)
        {
            return new(HairStyleId, newColor, FacialHairStyleId, FacialHairColor, EyeColor, SkinColor, Markings, SpeciesBase);
        }

        public HumanoidCharacterAppearance WithFacialHairStyleName(string newName)
        {
            return new(HairStyleId, HairColor, newName, FacialHairColor, EyeColor, SkinColor, Markings, SpeciesBase);
        }

        public HumanoidCharacterAppearance WithFacialHairColor(Color newColor)
        {
            return new(HairStyleId, HairColor, FacialHairStyleId, newColor, EyeColor, SkinColor, Markings, SpeciesBase);
        }

        public HumanoidCharacterAppearance WithEyeColor(Color newColor)
        {
            return new(HairStyleId, HairColor, FacialHairStyleId, FacialHairColor, newColor, SkinColor, Markings, SpeciesBase);
        }

        public HumanoidCharacterAppearance WithSkinColor(Color newColor)
        {
            return new(HairStyleId, HairColor, FacialHairStyleId, FacialHairColor, EyeColor, newColor, Markings, SpeciesBase);
        }

        // ANTHROSYSTEM MODIFICATION
        public HumanoidCharacterAppearance WithMarkings(List<AnthroMarking> newMarkings)
        {
            return new(HairStyleId, HairColor, FacialHairStyleId, FacialHairColor, EyeColor, SkinColor, newMarkings, SpeciesBase);
        }

        public HumanoidCharacterAppearance WithSpeciesBase(string newSpeciesBase)
        {
            return new(HairStyleId, HairColor, FacialHairStyleId, FacialHairColor, EyeColor, SkinColor, Markings, newSpeciesBase);
        }

        // ANTHROSYSTEM MODIFICATION


        public static HumanoidCharacterAppearance Default()
        {
            return new(
                HairStyles.DefaultHairStyle,
                Color.Black,
                HairStyles.DefaultFacialHairStyle,
                Color.Black,
                Color.Black,
                Color.FromHex("#C0967F"),
                // ANTHROSYSTEM MODIFICATION
                new List<AnthroMarking>(),
                AnthroSpeciesManager.DefaultBase
                // ANTHROSYSTEM MODIFICATION
            );
        }

        public static HumanoidCharacterAppearance Random(Sex sex)
        {
            var random = IoCManager.Resolve<IRobustRandom>();
            var prototypes = IoCManager.Resolve<SpriteAccessoryManager>();
            var hairStyles = prototypes.AccessoriesForCategory(SpriteAccessoryCategories.HumanHair);
            var facialHairStyles = prototypes.AccessoriesForCategory(SpriteAccessoryCategories.HumanHair);

            var newHairStyle = random.Pick(hairStyles).ID;

            var newFacialHairStyle = sex == Sex.Female
                ? HairStyles.DefaultFacialHairStyle
                : random.Pick(facialHairStyles).ID;

            var newHairColor = random.Pick(HairStyles.RealisticHairColors);
            newHairColor = newHairColor
                .WithRed(RandomizeColor(newHairColor.R))
                .WithGreen(RandomizeColor(newHairColor.G))
                .WithBlue(RandomizeColor(newHairColor.B));

            // TODO: Add random eye and skin color
            return new HumanoidCharacterAppearance(newHairStyle, newHairColor, newFacialHairStyle, newHairColor, Color.Black, Color.FromHex("#C0967F"), new List<AnthroMarking>(), AnthroSpeciesManager.DefaultBase);

            float RandomizeColor(float channel)
            {
                return MathHelper.Clamp01(channel + random.Next(-25, 25) / 100f);
            }
        }

        public static Color ClampColor(Color color)
        {
            return new(color.RByte, color.GByte, color.BByte);
        }

        public static HumanoidCharacterAppearance EnsureValid(HumanoidCharacterAppearance appearance)
        {
            var mgr = IoCManager.Resolve<SpriteAccessoryManager>();
            var hairStyleId = appearance.HairStyleId;
            if (!mgr.IsValidAccessoryInCategory(hairStyleId, SpriteAccessoryCategories.HumanHair))
            {
                hairStyleId = HairStyles.DefaultHairStyle;
            }

            var facialHairStyleId = appearance.FacialHairStyleId;
            if (!mgr.IsValidAccessoryInCategory(facialHairStyleId, SpriteAccessoryCategories.HumanFacialHair))
            {
                facialHairStyleId = HairStyles.DefaultFacialHairStyle;
            }

            var hairColor = ClampColor(appearance.HairColor);
            var facialHairColor = ClampColor(appearance.FacialHairColor);
            var eyeColor = ClampColor(appearance.EyeColor);
            var skinColor = ClampColor(appearance.SkinColor);

            // ANTHROSYSTEM MODIFICATION
            var markingManager = IoCManager.Resolve<AnthroMarkingManager>();
            List<AnthroMarking> validMarkings = new();
            foreach (var marking in appearance.Markings)
            {
                if (markingManager.IsValidMarking(marking.MarkingId, out AnthroMarkingPrototype? validMarkingPrototype))
                {
                    AnthroMarking validMarking = validMarkingPrototype.AsMarking();
                    validMarking.MarkingColor = marking.MarkingColor;
                    validMarkings.Add(validMarking);
                }
            }
            var speciesManager = IoCManager.Resolve<AnthroSpeciesManager>();
            var speciesBase = appearance.SpeciesBase;
            if (!speciesManager.SpeciesHasSprites(appearance.SpeciesBase, out var _))
               speciesBase = AnthroSpeciesManager.DefaultBase;
            // ANTHROSYSTEM MODIFICATION

            return new HumanoidCharacterAppearance(
                hairStyleId,
                hairColor,
                facialHairStyleId,
                facialHairColor,
                eyeColor,
                skinColor,
                validMarkings,
                speciesBase);
        }

        public bool MemberwiseEquals(ICharacterAppearance maybeOther)
        {
            if (maybeOther is not HumanoidCharacterAppearance other) return false;
            if (HairStyleId != other.HairStyleId) return false;
            if (!HairColor.Equals(other.HairColor)) return false;
            if (FacialHairStyleId != other.FacialHairStyleId) return false;
            if (!FacialHairColor.Equals(other.FacialHairColor)) return false;
            if (!EyeColor.Equals(other.EyeColor)) return false;
            if (!SkinColor.Equals(other.SkinColor)) return false;
            if (!_markings.SequenceEqual(other._markings)) return false;
            if (!SpeciesBase.Equals(other.SpeciesBase)) return false;
            return true;
        }
    }
}
