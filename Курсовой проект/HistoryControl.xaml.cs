using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Курсовой_проект
{
    public partial class HistoryControl : UserControl
    {
        public HistoryControl()
        {
            InitializeComponent();
            Loaded += HistoryControl_Loaded;
        }

        private void HistoryControl_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                HistoryScroll?.ScrollToEnd();
            }), DispatcherPriority.Loaded);
        }
    }
}