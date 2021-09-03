# FormFieldSaver
Library for saving data on the form after closing the program
## Supported Controls (System.Windows.Forms)
* TextBox
* NumericUpDown
* CheckBox
* ComboBox
* CheckedListBox
* DateTimePicker
* DomainUpDown
* ListBox
* RichTextBox
* TrackBar
* MaskedTextBox
## How to use
1. Install the Library in your WinForms Project
2. Add "**Load**" and "**FormClosing**" events to your Form<br>
![alt-image](https://i.imgur.com/EVasgjn.png)
3. Connect Using
   ```csharp
   using FormFieldSaver;
3. Call library methods in events
    ```csharp
    private void Form1_Load(object sender, EventArgs e)
    {
        // First parameter - Path to fiels data | Second parameter - Controls from your form
        FormSaver.LoadFormSettings("SavedFields.json", Controls.Cast<Control>().ToList());
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // First parameter - Path to fiels data | Second parameter - Controls from your form
        FormSaver.SaveFormSettings("SavedFields.json", Controls.Cast<Control>().ToList());
    }
## Ignoring some Сontrols
We have the ability to ignore some controls, this can be done by its type, or by its name
1. Ignoring by Type    
   ```csharp
   var ingnoreControls = new List<Type>();
   ingnoreControls.Add(typeof(CheckBox));
   ingnoreControls.Add(typeof(TextBox));

   // Now controls with the "CheckBox" and "TextBox" types will be ignored and not saved
   FormSaver.SaveFormSettings("SavedFields.json", Controls.Cast<Control>().ToList(), ingnoreControls);
2. Ignoring by Name 
   ```csharp
   var ignoreControlsByName = new List<string>();
   ignoreControlsByName.Add("checkBox1");
   ignoreControlsByName.Add("textBox1");

   // Now controls named "checkBox1" and "textBox1" will be ignored and not saved.
   FormSaver.SaveFormSettings("SavedFields.json", Controls.Cast<Control>().ToList(), null, ignoreControlsByName);
## Error Handling
When loading / saving fields, an error may occur, it is better to handle it
* When loading settings
   ```csharp
   var loadSettingsResult = FormSaver.LoadFormSettings("SavedFields.json", Controls.Cast<Control>().ToList());
   if(!loadSettingsResult.Success)
   {
       // Perhaps the program is launched for the first time, and it does not have a save file
       if (loadSettingsResult.FileNotFound) 
           return;

       // If the file exists, then an error occurred when working with controls, you need to prevent this.
       MessageBox.Show("Error load Form Settings", loadSettingsResult.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
* When save settings
   ```csharp
   var saveSettnigsResult = FormSaver.SaveFormSettings("SavedFields.json", Controls.Cast<Control>().ToList());
   if (!saveSettnigsResult.Success)
   {
       // an error occurred while working with controls, you need to prevent this.
       MessageBox.Show("Error load Form Settings", saveSettnigsResult.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
