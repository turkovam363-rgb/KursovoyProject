using System.Windows.Controls;
using System.Windows.Input;

namespace Курсовой_проект
{
    public partial class NovelControl : UserControl
    {
        public NovelControl()
        {
            InitializeComponent();
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ViewModels.NovelViewModel vm)
            {
                if (vm.NextDialogueCommand.CanExecute(null))
                {
                    vm.NextDialogueCommand.Execute(null);
                }
            }
        }
    }
}