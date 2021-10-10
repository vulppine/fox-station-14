using Content.Shared.AnthroSystem;
using Content.Shared.CharacterAppearance;
using Robust.Shared.IoC;

namespace Content.Shared.IoC
{
    public static class SharedContentIoC
    {
        public static void Register()
        {
            // ANTHROSYSTEM MODIFICATION
            IoCManager.Register<AnthroSpeciesManager, AnthroSpeciesManager>();
            IoCManager.Register<AnthroMarkingManager, AnthroMarkingManager>();
            // ANTHROSYSTEM MODIFICATION
            IoCManager.Register<SpriteAccessoryManager, SpriteAccessoryManager>();
        }
    }
}
