using System;
using System.Threading.Tasks;
using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Construction.Conditions
{
    [UsedImplicitly]
    [DataDefinition]
    public class AllConditions : IGraphCondition
    {
        [DataField("conditions")]
        public IGraphCondition[] Conditions { get; } = Array.Empty<IGraphCondition>();

        public async Task<bool> Condition(IEntity entity)
        {
            foreach (var condition in Conditions)
            {
                if (!await condition.Condition(entity))
                    return false;
            }

            return true;
        }

        public bool DoExamine(ExaminedEvent args)
        {
            var ret = false;

            foreach (var condition in Conditions)
            {
                ret |= condition.DoExamine(args);
            }

            return ret;
        }
    }
}
