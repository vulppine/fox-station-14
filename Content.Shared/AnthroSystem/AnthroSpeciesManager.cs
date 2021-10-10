using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Content.Shared.CharacterAppearance;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.AnthroSystem
{
    public sealed class AnthroSpeciesManager
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        private readonly Dictionary<AnthroSpeciesBase, Dictionary<HumanoidVisualLayers, SpriteSpecifier?>?> _index = new();

        public void Initialize()
        {
            foreach (var prototype in _prototypeManager.EnumeratePrototypes<AnthroSpeciesPrototype>())
            {
                foreach (var (layer, _) in prototype.SpeciesParts)
                    Logger.DebugS("ASM", $"{prototype.SpeciesBase} contains a reference for {layer}.");
                _index[prototype.SpeciesBase] = prototype.SpeciesParts;
            }
        }

        public bool SpeciesHasSprites(AnthroSpeciesBase species,
                [NotNullWhen(true)] out IReadOnlyCollection<KeyValuePair<HumanoidVisualLayers, SpriteSpecifier?>>? speciesParts)
        {
            speciesParts = _index[species];
            if (speciesParts is null) return false;

            return true;
        }
        /*
        public bool IsValidSpeciesSprite(AnthroSpeciesBase species, [NotNullWhen(true)] out SpriteSpecifier? speciesSprite)
        {
            switch (_index[species])
            {
                case SpriteSpecifier.Rsi sprite:
                    speciesSprite = sprite;
                    return true;
            }

            speciesSprite = null;
            return false;
        }
        */

        /* this is implicitly valid because speciesParts is always required
        public bool IsValidSpecies(string species, [NotNullWhen(true)] out Dictionary<HumanoidVisualLayers, SpriteSpecifier?>? speciesResult)
        {
            speciesResult = null;
            if (!Enum.TryParse(typeof(AnthroSpeciesBase), species, out var speciesBase)) return false;

        }
        */
    }
}
