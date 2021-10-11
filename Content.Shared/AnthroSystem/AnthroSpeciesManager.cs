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

        private readonly Dictionary<string, Dictionary<HumanoidVisualLayers, SpriteSpecifier?>?> _index = new();
        public static string DefaultBase = "human";

        public void Initialize()
        {
            foreach (var prototype in _prototypeManager.EnumeratePrototypes<AnthroSpeciesPrototype>())
            {
                foreach (var (layer, _) in prototype.SpeciesParts)
                    Logger.DebugS("ASM", $"{prototype.ID} contains a reference for {layer}.");
                _index[prototype.ID] = prototype.SpeciesParts;
            }
        }

        public List<string> AvailableSpecies() => new List<string>(_index.Keys);

        public bool SpeciesHasSprites(string species,
                [NotNullWhen(true)] out IReadOnlyCollection<KeyValuePair<HumanoidVisualLayers, SpriteSpecifier?>>? result)
        {
            result = null;
            _index.TryGetValue(species, out var speciesParts);
            if (speciesParts is null) return false;
            result = speciesParts;

            return true;
        }
    }
}
