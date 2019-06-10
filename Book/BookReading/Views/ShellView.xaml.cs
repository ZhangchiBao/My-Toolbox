using StyletIoC;
using System.Windows.Controls;

namespace BookReading.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView
    {
        public ShellView(IContainer container)
        {
            InitializeComponent();
        }
    }
}
