using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;

namespace Content.Shared.AnthroSystem
{
    public sealed class AnthroMarkingManager
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        private readonly List<AnthroMarkingPrototype> _index = new();

        public void Initialize()
        {
            _prototypeManager.PrototypesReloaded += OnPrototypeReload;

            foreach (var prototype in _prototypeManager.EnumeratePrototypes<AnthroMarkingPrototype>())
            {
                _index.Add(prototype);
            }

            // _index.Sort();
        }

        public IReadOnlyList<AnthroMarkingPrototype> Markings() => _index;

        // the most DEVIOUS lick
        // mostly because i seriously don't like the whole out thing, but whatever
        // TODO: O(n) to O(log n)
        public bool IsValidMarking(string marking, [NotNullWhen(true)] out AnthroMarkingPrototype? markingResult)
        {
            foreach (var markingPrototype in _index)
            {
                if (marking == markingPrototype.ID)
                {
                    markingResult = markingPrototype;
                    return true;
                }
            }

            markingResult = null;
            return false;
        }

        private void OnPrototypeReload(PrototypesReloadedEventArgs args)
        {
            if(!args.ByType.TryGetValue(typeof(AnthroMarkingPrototype), out var set))
                return;


            _index.RemoveAll(i => set.Modified.ContainsKey(i.ID));

            foreach (var prototype in set.Modified.Values)
            {
                var markingPrototype = (AnthroMarkingPrototype) prototype;
                _index.Add(markingPrototype);
            }
        }
    }
}
