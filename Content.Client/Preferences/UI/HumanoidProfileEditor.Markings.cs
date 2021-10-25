using System.Collections.Generic;
using System.Linq;
using Content.Client.AnthroSystem;
using Content.Shared.AnthroSystem;
using Content.Shared.Preferences;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Log;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Preferences.UI
{
    public partial class HumanoidProfileEditor
    {
        private readonly AnthroMarkingPicker _markingPicker;

        public static BoxContainer MarkingsTab(AnthroMarkingPicker markingPicker)
        {
            var markingsVBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
            };

            var markingsPanel = new HighlightedContainer();

            markingsPanel.AddChild(markingPicker);
            markingsVBox.AddChild(markingsPanel);

            return markingsVBox;
        }

        // really REALLY dirty way of doing this
        // maybe find a way to fix layer removal
        // and sprite rerender in RobustToolbox
        // very very very soon
        private void RecreateDummy(IEntityManager entityManager)
        {
            _previewDummy.Delete();
            _previewDummy = null!;
            _previewDummy = entityManager.SpawnEntity("MobHumanDummy", MapCoordinates.Nullspace);
            var sprite = _previewDummy.GetComponent<SpriteComponent>();
            _previewSpriteControl.RemoveAllChildren();
            _previewSpriteSideControl.RemoveAllChildren();
            _previewSpriteControl.AddChild(new SpriteView
            {
                Sprite = sprite,
                Scale = (6, 6),
                OverrideDirection = Direction.South,
                VerticalAlignment = VAlignment.Center,
                SizeFlagsStretchRatio = 1
            });
            _previewSpriteSideControl.AddChild(new SpriteView
            {
                Sprite = sprite,
                Scale = (6, 6),
                OverrideDirection = Direction.East,
                VerticalAlignment = VAlignment.Center,
                SizeFlagsStretchRatio = 1
            });
        }

        private void UpdateMarkings()
        {
            if (Profile is null)
                return;

            _markingPicker.SetData(Profile.Appearance.Markings.ToList(), Profile.Appearance.SkinColor, Profile.Appearance.SpeciesBase);
        }

    }
}
