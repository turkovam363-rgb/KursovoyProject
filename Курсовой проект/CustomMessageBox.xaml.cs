using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Курсовой_проект.Helpers;

namespace Курсовой_проект
{
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string title, string message)
        {
            InitializeComponent();

            TitleText.Text = title;
            MessageText.Text = message;

            if (Application.Current.MainWindow != null)
            {
                this.Owner = Application.Current.MainWindow;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ButtonSoundHelper.PlayClick();

            this.DialogResult = true;
            this.Close();
        }
    }
}
