using System;
using System.Collections.Generic;
using Content.Client.CharacterAppearance;
using Content.Client.Stylesheets;
using Content.Shared.AnthroSystem;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.Utility;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Maths;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.AnthroSystem
{
    public sealed class AnthroMarkingPicker : Control
    {
        [Dependency] private readonly AnthroMarkingManager _markingManager = default!;
        [Dependency] private readonly AnthroSpeciesManager _speciesManager = default!;

        // temporarily, as a treat
        // maybe use this information to create
        // a 'realistic' enum of skin colors?
        public Action<Color>? OnBodyColorChange;
        public Action<string>? OnSpeciesSelect;
        public Action<List<AnthroMarking>>? OnMarkingAdded;
        public Action<List<AnthroMarking>>? OnMarkingRemoved;
        public Action<List<AnthroMarking>>? OnMarkingColorChange;
        public Action<List<AnthroMarking>>? OnMarkingRankChange;

        private readonly ItemList _unusedMarkings;
        private readonly ItemList _usedMarkings;

        private readonly Control _bodyColorContainer;
        private readonly ColorSlider _bodyColorSliderR;
        private readonly ColorSlider _bodyColorSliderG;
        private readonly ColorSlider _bodyColorSliderB;


        private readonly Control _colorContainer;
        private readonly ColorSlider _colorSliderR;
        private readonly ColorSlider _colorSliderG;
        private readonly ColorSlider _colorSliderB;

        private readonly OptionButton _speciesButton;

        private readonly Button _addMarkingButton;
        private readonly Button _upRankMarkingButton;
        private readonly Button _downRankMarkingButton;
        private readonly Button _removeMarkingButton;

        private ItemList.Item? _selectedMarking;
        private ItemList.Item? _selectedUnusedMarking;
        private List<AnthroMarking> _usedMarkingList = new();
        private List<string> _availableSpecies = new();

        public void SetData(List<AnthroMarking> newMarkings, Color newBodyColor, string newSpecies)
        {
            _usedMarkingList = newMarkings;
            _usedMarkings.Clear();
            _selectedMarking = null;
            _selectedUnusedMarking = null;

            Logger.DebugS("AnthroMarkingSelector", $"New marking set: {_usedMarkingList}");
            foreach (var marking in _usedMarkingList)
            {
                if (_markingManager.IsValidMarking(marking.MarkingId, out AnthroMarkingPrototype? newMarking))
                {
                    var _item = _usedMarkings.AddItem(newMarking.ID, newMarking.Sprite.Frame0());
                    _item.Metadata = newMarking;
                    _item.IconModulate = marking.MarkingColor;
                }

                foreach (var unusedMarking in _unusedMarkings)
                {
                    if (unusedMarking.Metadata == newMarking)
                    {
                        _unusedMarkings.Remove(unusedMarking);
                        break;
                    }
                }
            }

            _bodyColorSliderR.ColorValue = newBodyColor.RByte;
            _bodyColorSliderG.ColorValue = newBodyColor.GByte;
            _bodyColorSliderB.ColorValue = newBodyColor.BByte;

            _speciesButton.SelectId(_availableSpecies.IndexOf(newSpecies));
        }

        public AnthroMarkingPicker()
        {
            IoCManager.InjectDependencies(this);

            var vBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical
            };
            AddChild(vBox);

            var speciesButtonContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                SeparationOverride = 5
            };
            _speciesButton = new OptionButton
            {
                HorizontalExpand = true
            };
            _availableSpecies = _speciesManager.AvailableSpecies();
            for (int i = 0; i < _availableSpecies.Count; i++)
                _speciesButton.AddItem(_availableSpecies[i], i);

            _speciesButton.OnItemSelected += args =>
            {
                _speciesButton.SelectId(args.Id);
                SetSpecies(_availableSpecies[args.Id]);
            };
            speciesButtonContainer.AddChild(new Label { Text = "Species sprite base:" });
            speciesButtonContainer.AddChild(_speciesButton);
            vBox.AddChild(speciesButtonContainer);


            // remember to remove this later
            _bodyColorContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
            };

            vBox.AddChild(_bodyColorContainer);
            _bodyColorContainer.AddChild(new Label { Text = "Current body color:" });
            _bodyColorContainer.AddChild(_bodyColorSliderR = new ColorSlider(StyleNano.StyleClassSliderRed));
            _bodyColorContainer.AddChild(_bodyColorSliderG = new ColorSlider(StyleNano.StyleClassSliderGreen));
            _bodyColorContainer.AddChild(_bodyColorSliderB = new ColorSlider(StyleNano.StyleClassSliderBlue));

            Action bodyColorChanged = BodyColorChanged;
            _bodyColorSliderR.OnValueChanged += bodyColorChanged;
            _bodyColorSliderG.OnValueChanged += bodyColorChanged;
            _bodyColorSliderB.OnValueChanged += bodyColorChanged;


            var markingListContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                SeparationOverride = 5
            };
            var unusedMarkingsContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical
            };
            _unusedMarkings = new ItemList
            {
                VerticalExpand = true,
                MinSize = (300, 250)
            };
            _unusedMarkings.OnItemSelected += item =>
               _selectedUnusedMarking = _unusedMarkings[item.ItemIndex];

            _addMarkingButton = new Button
            {
                Text = "Add Marking"
            };
            _addMarkingButton.OnPressed += args =>
                MarkingAdd();
            unusedMarkingsContainer.AddChild(_unusedMarkings);
            unusedMarkingsContainer.AddChild(_addMarkingButton);
            markingListContainer.AddChild(unusedMarkingsContainer);

            var usedMarkingsContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical
            };
            _usedMarkings = new ItemList
            {
                VerticalExpand = true,
                MinSize = (300, 250)
            };
            _usedMarkings.OnItemSelected += item =>
            {
               _selectedMarking = _usedMarkings[item.ItemIndex];
               _colorSliderR!.ColorValue = _selectedMarking.IconModulate.RByte;
               _colorSliderG!.ColorValue = _selectedMarking.IconModulate.GByte;
               _colorSliderB!.ColorValue = _selectedMarking.IconModulate.BByte;
               _colorContainer!.Visible = true;
            };

            var buttonRankingContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                SeparationOverride = 5
            };
            _upRankMarkingButton = new Button
            {
                Text = "Up",
                HorizontalExpand = true,
            };
            _downRankMarkingButton = new Button
            {
                Text = "Down",
                HorizontalExpand = true,
            };
            buttonRankingContainer.AddChild(_upRankMarkingButton);
            buttonRankingContainer.AddChild(_downRankMarkingButton);

            _removeMarkingButton = new Button
            {
                Text = "Remove Marking"
            };
            _removeMarkingButton.OnPressed += args =>
                MarkingRemove();

            usedMarkingsContainer.AddChild(_usedMarkings);
            usedMarkingsContainer.AddChild(buttonRankingContainer);
            usedMarkingsContainer.AddChild(_removeMarkingButton);
            markingListContainer.AddChild(usedMarkingsContainer);

            vBox.AddChild(markingListContainer);

            _colorContainer = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                Visible = false
            };
            vBox.AddChild(_colorContainer);
            _colorContainer.AddChild(new Label { Text = "Current marking color:" });
            _colorContainer.AddChild(_colorSliderR = new ColorSlider(StyleNano.StyleClassSliderRed));
            _colorContainer.AddChild(_colorSliderG = new ColorSlider(StyleNano.StyleClassSliderGreen));
            _colorContainer.AddChild(_colorSliderB = new ColorSlider(StyleNano.StyleClassSliderBlue));

            Action colorChanged = ColorChanged;
            _colorSliderR.OnValueChanged += colorChanged;
            _colorSliderG.OnValueChanged += colorChanged;
            _colorSliderB.OnValueChanged += colorChanged;


        }

        public void Populate()
        {
            var markings = _markingManager.Markings();
            foreach (var marking in markings)
            {
                Logger.DebugS("AnthroMarkingSelector", $"Adding marking {marking.ID}");
                var item = _unusedMarkings.AddItem($"{marking.ID} ({marking.BodyPart})", marking.Sprite.Frame0());
                item.Metadata = marking;
            }
        }

        private void SetSpecies(string species) => OnSpeciesSelect?.Invoke(species);

        private void BodyColorChanged()
        {
            var newColor = new Color(
                _bodyColorSliderR.ColorValue,
                _bodyColorSliderG.ColorValue,
                _bodyColorSliderB.ColorValue
            );

            OnBodyColorChange?.Invoke(newColor);
        }


        private void ColorChanged()
        {
            if (_selectedMarking is null) return;
            var markingPrototype = (AnthroMarkingPrototype) _selectedMarking.Metadata!;
            int markingIndex = _usedMarkingList.FindIndex(m => m.MarkingId == markingPrototype.ID);

            if (markingIndex < 0) return; // ???

            var newColor = new Color(
                _colorSliderR.ColorValue,
                _colorSliderG.ColorValue,
                _colorSliderB.ColorValue
            );

            _selectedMarking.IconModulate = newColor;
            _usedMarkingList[markingIndex].MarkingColor = newColor;
            OnMarkingColorChange?.Invoke(_usedMarkingList);
        }

        private void MarkingAdd()
        {
            if (_usedMarkingList is null || _selectedUnusedMarking is null) return;

            AnthroMarkingPrototype marking = (AnthroMarkingPrototype) _selectedUnusedMarking.Metadata!;
            Logger.DebugS("AnthroMarkingSelector", $"Adding marking {marking.ID} to character");
            _usedMarkingList.Add(marking.AsMarking());
            Logger.DebugS("AnthroMarkingSelector", $"{_usedMarkingList}");

            _unusedMarkings.Remove(_selectedUnusedMarking);
            var item = _usedMarkings.AddItem(marking.ID, marking.Sprite.Frame0());
            item.Metadata = marking;

            _selectedUnusedMarking = null;
            OnMarkingAdded?.Invoke(_usedMarkingList);
        }

        private void MarkingRemove()
        {
            if (_usedMarkingList is null || _selectedMarking is null) return;

            AnthroMarkingPrototype marking = (AnthroMarkingPrototype) _selectedMarking.Metadata!;
            _usedMarkingList.Remove(marking.AsMarking());
            _usedMarkings.Remove(_selectedMarking);

            var item = _unusedMarkings.AddItem(marking.ID, marking.Sprite.Frame0());
            item.Metadata = marking;
            _selectedMarking = null;
            _colorContainer.Visible = false;
            OnMarkingRemoved?.Invoke(_usedMarkingList);
        }
    }
}
