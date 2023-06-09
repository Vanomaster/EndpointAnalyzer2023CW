using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Gui.Common
{
    /// <summary>
    /// Interaction logic for SampleMessageDialog.xaml.
    /// </summary>
    public partial class MessageDialog : UserControl
    {
        private readonly ObservableCollection<string> buttonTextOptions = new ()
        {
            "Ок",
            "Понятно",
            "Ладно",
        };

        public MessageDialog(string headerText, string messageText)
        {
            InitializeComponent();
            HeaderText.Text = headerText;
            MainTextBlockContent.Text = messageText;
            OkButton.Content = GetRandomText();
        }

        private string GetRandomText()
        {
            var random = new Random();
            int index = random.Next(buttonTextOptions.Count);

            return buttonTextOptions[index];
        }
    }
}
