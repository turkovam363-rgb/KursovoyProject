using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсовой_проект.ViewModels;

namespace Курсовой_проект
{
    public partial class NovelControl : UserControl
    {
        public NovelControl()
        {
            InitializeComponent();
            this.PreviewKeyDown += NovelControl_PreviewKeyDown;
        }

        private void NovelControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is NovelViewModel vm)
            {
                if (e.Key == Key.Space || e.Key == Key.Return || e.Key == Key.Enter)
                {
                    if (vm.NextDialogueCommand.CanExecute(null))
                    {
                        vm.NextDialogueCommand.Execute(null);
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.H)
                {
                    if (vm.HistoryCommand.CanExecute(null))
                    {
                        vm.HistoryCommand.Execute(null);
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.None)
                {
                    if (vm.SaveCommand.CanExecute(null))
                    {
                        vm.SaveCommand.Execute(null);
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.L)
                {
                    if (vm.OpenLoadMenuCommand.CanExecute(null))
                    {
                        vm.OpenLoadMenuCommand.Execute(null);
                        e.Handled = true;
                    }
                }
                else if (e.Key == Key.Escape)
                {
                    if (vm.ReturnToMenuCommand.CanExecute(null))
                    {
                        vm.ReturnToMenuCommand.Execute(null);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}