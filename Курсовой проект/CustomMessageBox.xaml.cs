using System.Windows;

namespace Курсовой_проект
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string title, string message)
        {
            InitializeComponent();

            TitleText.Text = title;
            MessageText.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}