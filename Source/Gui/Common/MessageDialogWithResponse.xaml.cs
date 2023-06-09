using System.Windows.Controls;

namespace Gui.Common
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class MessageDialogWithResponse : UserControl
    {
        public MessageDialogWithResponse(string headerText, string messageText)
        {
            InitializeComponent();
            HeaderText.Text = headerText;
            MainTextBlockContent.Text = messageText;
        }
    }
}
