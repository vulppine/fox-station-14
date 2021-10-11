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

        // temporarily, as a treat
        // maybe use this information to create
        // a 'realistic' enum of skin colors?
        public Action<Color>? OnBodyColorChange;
        public Action<AnthroSpeciesBase>? OnSpeciesSelect;
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

        // can there be something else other than this???
        // maybe make a whole ass drop down menu or
        // whatever, Not This Shit
        private List<Button> _speciesButtons = new();
        private readonly Button _addMarkingButton;
        private readonly Button _upRankMarkingButton;
        private readonly Button _downRankMarkingButton;
        private readonly Button _removeMarkingButton;

        private List<Color> _currentMarkingColors = new();

        private ItemList.Item? _selectedMarking;
        private ItemList.Item? _selectedUnusedMarking;
        private List<AnthroMarking> _usedMarkingList = new();

        public void SetData(List<AnthroMarking> newMarkings, Color newBodyColor)
        {
            _usedMarkingList = newMarkings;
            _usedMarkings.Clear();
            _selectedMarking = null;
            _selectedUnusedMarking = null;

            Logger.DebugS("AnthroMarkingSelector", $"New marking set: {_usedMarkingList}");
            // foreach (var m in _usedMarkingList)
            for (int i = 0; i < _usedMarkingList.Count; i++)
            {
                AnthroMarking marking = _usedMarkingList[i];
                if (_markingManager.IsValidMarking(marking, out AnthroMarkingPrototype? newMarking))
                {
                    // TODO: Composite sprite preview, somehow.
                    var _item = _usedMarkings.AddItem(newMarking.ID, newMarking.Sprites[0].Frame0());
                    _item.Metadata = newMarking;
                    _item.IconModulate = marking.MarkingColors[0];
                    if (marking.MarkingColors.Count != _usedMarkingList[i].MarkingColors.Count)
                    {
                        _usedMarkingList[i] = new AnthroMarking(marking.MarkingId, marking.MarkingColors);
                    }
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
            foreach (var species in Enum.GetValues<AnthroSpeciesBase>())
            {
                var button = new Button
                {
                    Text = species.ToString(),
                    HorizontalExpand = true
                };
                button.OnPressed += args =>
                    SetSpecies(species);
                _speciesButtons.Add(button);
                speciesButtonContainer.AddChild(button);
            }
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
            _usedMarkings.OnItemSelected += OnUsedMarkingSelected;

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
        }

        public void Populate()
        {
            var markings = _markingManager.Markings();
            foreach (var marking in markings)
            {
                Logger.DebugS("AnthroMarkingSelector", $"Adding marking {marking.ID}");
                var item = _unusedMarkings.AddItem($"{marking.ID} ({marking.BodyPart})", marking.Sprites[0].Frame0());
                item.Metadata = marking;
            }
        }

        private void OnUsedMarkingSelected(ItemList.ItemListSelectedEventArgs item)
        {
            _selectedMarking = _usedMarkings[item.ItemIndex];
            var prototype = (AnthroMarkingPrototype) _selectedMarking.Metadata!;
            _currentMarkingColors.Clear();
            _colorContainer.RemoveAllChildren();
            List<List<ColorSlider>> colorSliders = new();
            for (int i = 0; i < prototype.Sprites.Count; i++)
            {
                var colorContainer = new BoxContainer
                {
                    Orientation = LayoutOrientation.Vertical,
                };

                _colorContainer.AddChild(colorContainer);

                List<ColorSlider> sliders = new();
                ColorSlider colorSliderR = new ColorSlider(StyleNano.StyleClassSliderRed);
                ColorSlider colorSliderG = new ColorSlider(StyleNano.StyleClassSliderGreen);
                ColorSlider colorSliderB = new ColorSlider(StyleNano.StyleClassSliderBlue);

                colorContainer.AddChild(new Label { Text = $"{prototype.MarkingPartNames[i]} color:" });
                colorContainer.AddChild(colorSliderR);
                colorContainer.AddChild(colorSliderG);
                colorContainer.AddChild(colorSliderB);

                var currentColor = new Color(
                    _usedMarkingList[item.ItemIndex].MarkingColors[i].RByte,
                    _usedMarkingList[item.ItemIndex].MarkingColors[i].GByte,
                    _usedMarkingList[item.ItemIndex].MarkingColors[i].BByte
                );
                _currentMarkingColors.Add(currentColor);
                int colorIndex = _currentMarkingColors.IndexOf(currentColor);

                colorSliderR.ColorValue = currentColor.RByte;
                colorSliderG.ColorValue = currentColor.GByte;
                colorSliderB.ColorValue = currentColor.BByte;

                Action colorChanged = delegate()
                {
                    _currentMarkingColors[colorIndex] = new Color(
                        colorSliderR.ColorValue,
                        colorSliderG.ColorValue,
                        colorSliderB.ColorValue
                    );

                    ColorChanged(colorIndex);
                };
                colorSliderR.OnValueChanged += colorChanged;
                colorSliderG.OnValueChanged += colorChanged;
                colorSliderB.OnValueChanged += colorChanged;
            }

            _colorContainer.Visible = true;
        }

        private void SetSpecies(AnthroSpeciesBase species) => OnSpeciesSelect?.Invoke(species);

        private void BodyColorChanged()
        {
            var newColor = new Color(
                _bodyColorSliderR.ColorValue,
                _bodyColorSliderG.ColorValue,
                _bodyColorSliderB.ColorValue
            );

            OnBodyColorChange?.Invoke(newColor);
        }


        private void ColorChanged(int colorIndex)
        {
            if (_selectedMarking is null) return;
            var markingPrototype = (AnthroMarkingPrototype) _selectedMarking.Metadata!;
            int markingIndex = _usedMarkingList.FindIndex(m => m.MarkingId == markingPrototype.ID);

            if (markingIndex < 0) return; // ???

            /*
            var newColor = new Color(
                _selectedMarkingColor[0].ColorValue,
                _selectedMarkingColor[1].ColorValue,
                _selectedMarkingColor[2].ColorValue
            );
            */

            _selectedMarking.IconModulate = _currentMarkingColors[colorIndex];
            _usedMarkingList[markingIndex].SetColor(colorIndex, _currentMarkingColors[colorIndex]);
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
            var item = _usedMarkings.AddItem(marking.ID, marking.Sprites[0].Frame0());
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

            var item = _unusedMarkings.AddItem(marking.ID, marking.Sprites[0].Frame0());
            item.Metadata = marking;
            _selectedMarking = null;
            _colorContainer.Visible = false;
            OnMarkingRemoved?.Invoke(_usedMarkingList);
        }
    }
}
