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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResourceSearcher.UI.Controls
{
    /// <summary>
    /// CTabItem.xaml 的交互逻辑
    /// </summary>
    public partial class CTabItem : TabItem
    {
        public CTabItem()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            var parent = FindParentTabControl(this);
            parent.Items.Remove(this);
        }

        /// <summary>
        /// 递归找父级TabControl
        /// </summary>
        /// <param name="reference">依赖对象</param>
        /// <returns>TabControl</returns>
        private TabControl FindParentTabControl(DependencyObject reference)
        {
            DependencyObject dObj = VisualTreeHelper.GetParent(reference);
            if (dObj == null)
                return null;
            if (dObj.GetType() == typeof(TabControl))
                return dObj as TabControl;
            else
                return FindParentTabControl(dObj);
        }
    }
}
