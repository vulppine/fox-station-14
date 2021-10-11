using System.Collections.Generic;
using System.Linq;
using Content.Client.AnthroSystem;
using Content.Shared.AnthroSystem;
using Content.Shared.Preferences;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
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

            var markingsPanel = HighlightedContainer();

            markingsPanel.AddChild(markingPicker);
            markingsVBox.AddChild(markingsPanel);

            return markingsVBox;
        }

        private void UpdateMarkings()
        {
            if (Profile is null)
                return;

            _markingPicker.SetData(Profile.Appearance.Markings.ToList(), Profile.Appearance.SkinColor, Profile.Appearance.SpeciesBase);
        }

    }
}
