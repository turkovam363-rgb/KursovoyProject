using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Курсовой_проект
{
    public partial class NovelControl : UserControl
    {
        public NovelControl()
        {
            InitializeComponent();
            Loaded += NovelControl_Loaded;
        }

        private void NovelControl_Loaded(object sender, RoutedEventArgs e)
        {
            Focus();
            Keyboard.Focus(this);
        }
    }
}