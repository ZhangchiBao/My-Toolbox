using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ResourceSearcher.UI.Views
{
    /// <summary>
    /// ShellPage.xaml 的交互逻辑
    /// </summary>
    public partial class ShellPage
    {
        public ShellPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock block)
            {
                dynamic data = block.DataContext;
                System.Diagnostics.Process.Start(data.Link);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                dynamic data = btn.DataContext;
                Clipboard.SetDataObject(data.Link);
            }
        }
    }
}
