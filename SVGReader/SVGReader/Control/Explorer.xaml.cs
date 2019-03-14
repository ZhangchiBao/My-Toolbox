using SVGReader.Entity;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace SVGReader.Control
{
    /// <summary>
    /// Explorer.xaml 的交互逻辑
    /// </summary>
    public partial class Explorer
    {
        public Explorer()
        {
            InitializeComponent();
            FilesTree.SelectedItemChanged += FilesTree_SelectedItemChanged;
            //FilesTree.SetBinding(TreeView.ItemsSourceProperty, new Binding(nameof(ItemsSource)));
            DataContext = this;
            FillRootLevel();
        }

        private void FilesTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is FloderEntity entity)
            {
                var directories = Directory.GetDirectories(entity.Path);
                foreach (var item in directories)
                {
                    if ((new DirectoryInfo(item).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        continue;
                    }

                    entity.Children.Add(new FloderEntity { Path = item });
                }
            }
        }

        public ObservableCollection<FloderEntity> ItemsSource
        {
            get { return (ObservableCollection<FloderEntity>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<FloderEntity>), typeof(Explorer), new PropertyMetadata(new ObservableCollection<FloderEntity>()));

        private void FillRootLevel()
        {
            foreach (var drive in Directory.GetLogicalDrives())
            {
                ItemsSource.Add(new FloderEntity { Path = drive });
            }
        }

        public FloderEntity SelectedItem
        {
            get { return (FloderEntity)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(FloderEntity), typeof(Explorer), new PropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
