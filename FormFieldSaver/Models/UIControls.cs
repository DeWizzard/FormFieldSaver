using System;
using System.Collections.Generic;

namespace FormFieldSaver.Models
{
    internal class UIControls
    {
        public List<UITextBox> TextBoxes { get; set; }
        public List<UIComboBox> ComboBoxes { get; set; }
        public List<UINumericUpDown> NumericUpDowns { get; set; }
        public List<UICheckBox> CheckBoxes { get; set; }
        public List<UICheckedListBox> CheckedListBoxes { get; set; }
        public List<UIDateTimePicker> UIDateTimePickers { get; set; }
        public List<UIDomainUpDown> DomainUpDowns { get; set; }
        public List<UIListBox> ListBoxes { get; set; }
        public List<UIRichTextBox> RichTextBoxes { get; set; }
        public List<UITrackBar> TrackBars { get; set; }
        public List<UIMaskedTextBox> MaskedTextBoxes { get; set; }

        public void Initializate()
        {
            TextBoxes = new List<UITextBox>();
            ComboBoxes = new List<UIComboBox>();
            NumericUpDowns = new List<UINumericUpDown>();
            CheckBoxes = new List<UICheckBox>();
            CheckedListBoxes = new List<UICheckedListBox>();
            UIDateTimePickers = new List<UIDateTimePicker>();
            DomainUpDowns = new List<UIDomainUpDown>();
            ListBoxes = new List<UIListBox>();
            RichTextBoxes = new List<UIRichTextBox>();
            TrackBars = new List<UITrackBar>();
            MaskedTextBoxes = new List<UIMaskedTextBox>();
        }
    }

    public class UITextBox
    {
        public string ControlName { get; set; }
        public string Text { get; set; }
    }

    public class UIComboBox
    {
        public string ControlName { get; set; }
        public int SelectedIndex { get; set; }
    }

    public class UINumericUpDown
    {
        public string ControlName { get; set; }
        public decimal Value { get; set; }
    }

    public class UICheckBox
    {
        public string ControlName { get; set; }
        public bool Checked { get; set; }
    }

    public class UICheckedListBox
    {
        public string ControlName { get; set; }
        public List<int> CheckedIndexes { get; set; }
    }

    public class UIDateTimePicker
    {
        public string ControlName { get; set; }
        public DateTime Value { get; set; }
    }

    public class UIDomainUpDown
    {
        public string ControlName { get; set; }
        public int SelectedIndex { get; set; }
    }

    public class UIListBox
    {
        public string ControlName { get; set; }
        public int SelectedIndex { get; set; }
    }

    public class UIRichTextBox
    {
        public string ControlName { get; set; }
        public string Text { get; set; }
    }

    public class UITrackBar
    {
        public string ControlName { get; set; }
        public int Value { get; set; }
    }

    public class UIMaskedTextBox
    {
        public string ControlName { get; set; }
        public string Text { get; set; }
    }
}
