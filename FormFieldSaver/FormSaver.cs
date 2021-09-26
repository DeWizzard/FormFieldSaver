using FormFieldSaver.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormFieldSaver
{
    public class FormSaver
    {
        private static bool CheckControlByType(List<Type> ignoreList, Type type)
        {
            if (type != null && ignoreList != null)
                foreach (var item in ignoreList)
                    if (item == type)
                        return true;

            return false;
        }

        private static bool CheckControlByName(List<string> ignoreList, string name)
        {
            if (name != null && ignoreList != null)
                foreach (var item in ignoreList)
                    if (item == name)
                        return true;

            return false;
        }

        private static bool FindControlIndex(List<Control> formControls, string name, Type type, ref int index)
        {
            index = formControls.FindIndex(w => w.Name == name && w.GetType() == type);
            if (index != -1)
                return true;

            return false;
        }

        private static IEnumerable<Control> GetControlsByName(Control form, string name, Type type)
        {
            var controls = form.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetControlsByType(ctrl, type))
                    .Concat(controls)
                    .Where(c => c.GetType() == type &&
                    c.Name == name);
        }

        private static IEnumerable<Control> GetControlsByType(Control control, Type type, List<Type> ignoreControls = null, List<string> ignoreControlsByName = null)
        {
            ignoreControls = (ignoreControls is null) ? new List<Type>() : ignoreControls;
            ignoreControlsByName = (ignoreControlsByName is null) ? new List<string>() : ignoreControlsByName;

            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetControlsByType(ctrl, type, ignoreControls, ignoreControlsByName))
                    .Concat(controls)
                    .Where(c => c.GetType() == type &&
                    !ignoreControls.Contains(c.GetType()) &&
                    !ignoreControlsByName.Contains(c.Name));
        }

        public static SaveFormSettingsInfo SaveFormSettings(string pathToFile, Control form, List<Type> ignoreControls = null, List<string> ignoreControlsByName = null)
        {
            try
            {
                var json = new UIControls();
                json.Initializate();

                foreach (TextBox textBox in GetControlsByType(form, typeof(TextBox), ignoreControls, ignoreControlsByName))
                    json.TextBoxes.Add(new UITextBox { ControlName = textBox.Name, Text = textBox.Text });

                foreach (NumericUpDown numericUpDown in GetControlsByType(form, typeof(NumericUpDown), ignoreControls, ignoreControlsByName))
                    json.NumericUpDowns.Add(new UINumericUpDown { ControlName = numericUpDown.Name, Value = numericUpDown.Value });

                foreach (CheckBox checkBox in GetControlsByType(form, typeof(CheckBox), ignoreControls, ignoreControlsByName))
                    json.CheckBoxes.Add(new UICheckBox { ControlName = checkBox.Name, Checked = checkBox.Checked });

                foreach (ComboBox comboBox in GetControlsByType(form, typeof(ComboBox), ignoreControls, ignoreControlsByName))
                    json.ComboBoxes.Add(new UIComboBox { ControlName = comboBox.Name, SelectedIndex = comboBox.SelectedIndex });

                foreach (CheckedListBox checkedListBox in GetControlsByType(form, typeof(CheckedListBox), ignoreControls, ignoreControlsByName))
                    json.CheckedListBoxes.Add(new UICheckedListBox { ControlName = checkedListBox.Name, CheckedIndexes = checkedListBox.CheckedIndices.Cast<int>().ToList() });

                foreach (DateTimePicker dateTimePicker in GetControlsByType(form, typeof(DateTimePicker), ignoreControls, ignoreControlsByName))
                    json.UIDateTimePickers.Add(new UIDateTimePicker { ControlName = dateTimePicker.Name, Value = dateTimePicker.Value });

                foreach (DomainUpDown domainUpDown in GetControlsByType(form, typeof(DomainUpDown), ignoreControls, ignoreControlsByName))
                    json.DomainUpDowns.Add(new UIDomainUpDown { ControlName = domainUpDown.Name, SelectedIndex = domainUpDown.SelectedIndex });

                foreach (ListBox listBox in GetControlsByType(form, typeof(ListBox), ignoreControls, ignoreControlsByName))
                    json.ListBoxes.Add(new UIListBox { ControlName = listBox.Name, SelectedIndex = listBox.SelectedIndex });

                foreach (RichTextBox richTextBox in GetControlsByType(form, typeof(RichTextBox), ignoreControls, ignoreControlsByName))
                    json.RichTextBoxes.Add(new UIRichTextBox { ControlName = richTextBox.Name, Text = richTextBox.Text });

                foreach (TrackBar trackBar in GetControlsByType(form, typeof(TrackBar), ignoreControls, ignoreControlsByName))
                    json.TrackBars.Add(new UITrackBar { ControlName = trackBar.Name, Value = trackBar.Value });

                foreach (MaskedTextBox maskedTextBox in GetControlsByType(form, typeof(MaskedTextBox), ignoreControls, ignoreControlsByName))
                    json.MaskedTextBoxes.Add(new UIMaskedTextBox { ControlName = maskedTextBox.Name, Text = maskedTextBox.Text });

                var serializeJson = JsonConvert.SerializeObject(json, Formatting.Indented);
                File.WriteAllText(pathToFile, serializeJson, Encoding.UTF8);

                return new SaveFormSettingsInfo { Success = true };
            }
            catch (Exception ex)
            {
                return new SaveFormSettingsInfo { Success = false, Error = ex };
            }
        }

        public static LoadFormSettingsInfo LoadFormSettings(string pathToFile, Control form)
        {
            try
            {
                if (!File.Exists(pathToFile))
                    return new LoadFormSettingsInfo { Success = false, FileNotFound = true };

                var formSettings = File.ReadAllText(pathToFile, Encoding.UTF8);
                var uIControls = JsonConvert.DeserializeObject<UIControls>(formSettings);

                foreach (var control in uIControls.TextBoxes ?? Enumerable.Empty<UITextBox>())
                    foreach (TextBox textBox in GetControlsByName(form, control.ControlName, typeof(TextBox)))
                        textBox.Text = control.Text;

                foreach (var control in uIControls.NumericUpDowns ?? Enumerable.Empty<UINumericUpDown>())
                    foreach (NumericUpDown numericUpDown in GetControlsByName(form, control.ControlName, typeof(NumericUpDown)))
                        numericUpDown.Value = control.Value;

                foreach (var control in uIControls.CheckBoxes ?? Enumerable.Empty<UICheckBox>())
                    foreach (CheckBox checkBox in GetControlsByName(form, control.ControlName, typeof(CheckBox)))
                        checkBox.Checked = control.Checked;

                foreach (var control in uIControls.ComboBoxes ?? Enumerable.Empty<UIComboBox>())
                    foreach (ComboBox comboBox in GetControlsByName(form, control.ControlName, typeof(ComboBox)))
                        comboBox.SelectedIndex = control.SelectedIndex;

                foreach (var control in uIControls.CheckedListBoxes ?? Enumerable.Empty<UICheckedListBox>())
                    foreach (CheckedListBox checkedListBox in GetControlsByName(form, control.ControlName, typeof(CheckedListBox)))
                        foreach (var checkedIndex in control.CheckedIndexes)
                            checkedListBox.SetItemChecked(checkedIndex, true);

                foreach (var control in uIControls.UIDateTimePickers ?? Enumerable.Empty<UIDateTimePicker>())
                    foreach (DateTimePicker dateTimePicker in GetControlsByName(form, control.ControlName, typeof(DateTimePicker)))
                        dateTimePicker.Value = control.Value;

                foreach (var control in uIControls.DomainUpDowns ?? Enumerable.Empty<UIDomainUpDown>())
                    foreach (DomainUpDown domainUpDown in GetControlsByName(form, control.ControlName, typeof(DomainUpDown)))
                        domainUpDown.SelectedIndex = control.SelectedIndex;

                foreach (var control in uIControls.ListBoxes ?? Enumerable.Empty<UIListBox>())
                    foreach (ListBox listBox in GetControlsByName(form, control.ControlName, typeof(ListBox)))
                        listBox.SelectedIndex = control.SelectedIndex;

                foreach (var control in uIControls.RichTextBoxes ?? Enumerable.Empty<UIRichTextBox>())
                    foreach (RichTextBox richTextBox in GetControlsByName(form, control.ControlName, typeof(RichTextBox)))
                        richTextBox.Text = control.Text;

                foreach (var control in uIControls.TrackBars ?? Enumerable.Empty<UITrackBar>())
                    foreach (TrackBar trackBar in GetControlsByName(form, control.ControlName, typeof(TrackBar)))
                        trackBar.Value = control.Value;

                foreach (var control in uIControls.MaskedTextBoxes ?? Enumerable.Empty<UIMaskedTextBox>())
                    foreach (MaskedTextBox maskedTextBox in GetControlsByName(form, control.ControlName, typeof(MaskedTextBox)))
                        maskedTextBox.Text = control.Text;

                return new LoadFormSettingsInfo { Success = true };
            }
            catch (Exception ex)
            {
                return new LoadFormSettingsInfo { Success = false, Error = ex };
            }
        }

        [Obsolete("This method is deprecated and does not work correctly, use a different method overload, more details: https://github.com/DeWizzard/FormFieldSaver")]
        public static SaveFormSettingsInfo SaveFormSettings(string pathToFile, List<Control> formControls, List<Type> ignoreControls = null, List<string> ignoreControlsByName = null)
        {
            try
            {
                var json = new UIControls();
                json.Initializate();

                if (ignoreControls != null)
                    formControls.RemoveAll(w => CheckControlByType(ignoreControls, w.GetType()));

                if (ignoreControlsByName != null)
                    formControls.RemoveAll(w => CheckControlByName(ignoreControlsByName, w.Name));

                foreach (var control in formControls.OfType<TextBox>())
                    json.TextBoxes.Add(new UITextBox { ControlName = control.Name, Text = control.Text });

                foreach (var control in formControls.OfType<NumericUpDown>())
                    json.NumericUpDowns.Add(new UINumericUpDown { ControlName = control.Name, Value = control.Value });

                foreach (var control in formControls.OfType<CheckBox>())
                    json.CheckBoxes.Add(new UICheckBox { ControlName = control.Name, Checked = control.Checked });

                foreach (var control in formControls.OfType<ComboBox>())
                    json.ComboBoxes.Add(new UIComboBox { ControlName = control.Name, SelectedIndex = control.SelectedIndex });

                foreach (var control in formControls.OfType<CheckedListBox>())
                    json.CheckedListBoxes.Add(new UICheckedListBox { ControlName = control.Name, CheckedIndexes = control.CheckedIndices.Cast<int>().ToList() });

                foreach (var control in formControls.OfType<DateTimePicker>())
                    json.UIDateTimePickers.Add(new UIDateTimePicker { ControlName = control.Name, Value = control.Value });

                foreach (var control in formControls.OfType<DomainUpDown>())
                    json.DomainUpDowns.Add(new UIDomainUpDown { ControlName = control.Name, SelectedIndex = control.SelectedIndex });

                foreach (var control in formControls.OfType<ListBox>())
                    json.ListBoxes.Add(new UIListBox { ControlName = control.Name, SelectedIndex = control.SelectedIndex });

                foreach (var control in formControls.OfType<RichTextBox>())
                    json.RichTextBoxes.Add(new UIRichTextBox { ControlName = control.Name, Text = control.Text });

                foreach (var control in formControls.OfType<TrackBar>())
                    json.TrackBars.Add(new UITrackBar { ControlName = control.Name, Value = control.Value });

                foreach (var control in formControls.OfType<MaskedTextBox>())
                    json.MaskedTextBoxes.Add(new UIMaskedTextBox { ControlName = control.Name, Text = control.Text });

                var serializeJson = JsonConvert.SerializeObject(json, Formatting.Indented);
                File.WriteAllText(pathToFile, serializeJson, Encoding.UTF8);

                return new SaveFormSettingsInfo { Success = true };
            }
            catch (Exception ex)
            {
                return new SaveFormSettingsInfo { Success = false, Error = ex };
            }
        }

        [Obsolete("This method is deprecated and does not work correctly, use a different method overload, more details: https://github.com/DeWizzard/FormFieldSaver")]
        public static LoadFormSettingsInfo LoadFormSettings(string pathToFile, List<Control> formControls)
        {
            try
            {
                if (!File.Exists(pathToFile))
                    return new LoadFormSettingsInfo { Success = false, FileNotFound = true };

                var formSettings = File.ReadAllText(pathToFile, Encoding.UTF8);
                var uIControls = JsonConvert.DeserializeObject<UIControls>(formSettings);
                var index = -1;

                foreach (var control in uIControls.TextBoxes ?? Enumerable.Empty<UITextBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(TextBox), ref index))
                        ((TextBox)formControls[index]).Text = control.Text;

                foreach (var control in uIControls.NumericUpDowns ?? Enumerable.Empty<UINumericUpDown>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(NumericUpDown), ref index))
                        ((NumericUpDown)formControls[index]).Value = control.Value;

                foreach (var control in uIControls.CheckBoxes ?? Enumerable.Empty<UICheckBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(CheckBox), ref index))
                        ((CheckBox)formControls[index]).Checked = control.Checked;

                foreach (var control in uIControls.ComboBoxes ?? Enumerable.Empty<UIComboBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(ComboBox), ref index))
                        ((ComboBox)formControls[index]).SelectedIndex = control.SelectedIndex;

                foreach (var control in uIControls.CheckedListBoxes ?? Enumerable.Empty<UICheckedListBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(CheckedListBox), ref index))
                        foreach (var checkedIndex in control.CheckedIndexes)
                            ((CheckedListBox)formControls[index]).SetItemChecked(checkedIndex, true);

                foreach (var control in uIControls?.UIDateTimePickers ?? Enumerable.Empty<UIDateTimePicker>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(DateTimePicker), ref index))
                        ((DateTimePicker)formControls[index]).Value = control.Value;

                foreach (var control in uIControls.DomainUpDowns ?? Enumerable.Empty<UIDomainUpDown>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(DomainUpDown), ref index))
                        ((DomainUpDown)formControls[index]).SelectedIndex = control.SelectedIndex;

                foreach (var control in uIControls.ListBoxes ?? Enumerable.Empty<UIListBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(ListBox), ref index))
                        ((ListBox)formControls[index]).SelectedIndex = control.SelectedIndex;

                foreach (var control in uIControls.RichTextBoxes ?? Enumerable.Empty<UIRichTextBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(RichTextBox), ref index))
                        ((RichTextBox)formControls[index]).Text = control.Text;

                foreach (var control in uIControls.TrackBars ?? Enumerable.Empty<UITrackBar>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(TrackBar), ref index))
                        ((TrackBar)formControls[index]).Value = control.Value;

                foreach (var control in uIControls.MaskedTextBoxes ?? Enumerable.Empty<UIMaskedTextBox>())
                    if (FindControlIndex(formControls, control.ControlName, typeof(MaskedTextBox), ref index))
                        ((MaskedTextBox)formControls[index]).Text = control.Text;

                return new LoadFormSettingsInfo { Success = true };
            }
            catch (Exception ex)
            {
                return new LoadFormSettingsInfo { Success = false, Error = ex };
            }
        }
    }
}
