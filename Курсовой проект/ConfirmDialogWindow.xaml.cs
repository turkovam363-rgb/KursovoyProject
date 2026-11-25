using System;
using System.Windows;

namespace Курсовой_проект
{
    public partial class ConfirmDialogWindow : Window
    {
        public ConfirmDialogWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message ?? string.Empty;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWithResult(true);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWithResult(false);
        }

        private void CloseWithResult(bool result)
        {
            try
            {
                DialogResult = result;
            }
            catch (InvalidOperationException)
            {
                Close();
            }
        }
    }
}