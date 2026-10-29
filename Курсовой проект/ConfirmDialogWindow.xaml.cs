using System.Windows;

namespace Курсовой_проект
{
    public partial class ConfirmDialogWindow : Window
    {
        public ConfirmDialogWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}