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
![alt-image](https://i.imgur.com/EVasgjn.png "Текст заголовка логотипа 1")
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
