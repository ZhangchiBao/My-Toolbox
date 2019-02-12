using Book.Models;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class SitesSettingViewModel : Screen
    {
        private readonly IViewManager viewManager;
        private readonly IContainer container;

        public SitesSettingViewModel(IViewManager viewManager, IContainer container)
        {
            this.viewManager = viewManager;
            this.container = container;
        }

        /// <summary>
        /// 站点清单
        /// </summary>
        public ObservableCollection<SiteInfo> Sites { get; set; } = new ObservableCollection<SiteInfo>();

        /// <summary>
        /// 当前选中站点
        /// </summary>
        public SiteInfo SelectedSite { get; set; }

        /// <summary>
        /// 能删除站点
        /// </summary>
        public bool CanDeleteSite => SelectedSite != null;

        /// <summary>
        /// 添加站点
        /// </summary>
        public void AddSite()
        {
            var site = new SiteInfo();
            var siteDetailViewModel = container.Get<SiteDetailViewModel>();
            siteDetailViewModel.Site = site;
            var siteDetailView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(siteDetailViewModel);
            siteDetailView.Owner = (Window)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>());
            siteDetailView.ShowDialog();
        }

        /// <summary>
        /// 删除站点
        /// </summary>
        public void DeleteSite()
        {
            Sites.Remove(SelectedSite);
        }

        /// <summary>
        /// 站点列表被双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SitesGridDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)e.OriginalSource).DataContext is SiteInfo site)
            {
                var siteDetailViewModel = container.Get<SiteDetailViewModel>();
                siteDetailViewModel.Site = site;
                var siteDetailView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(siteDetailViewModel);
                siteDetailView.Owner = (Window)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>());
                siteDetailView.ShowDialog();
            }
        }

        public void SitesGridRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(((FrameworkElement)e.OriginalSource).DataContext is SiteInfo))
            {
                SelectedSite = null;
            }
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        public void CloseDialog()
        {
            var dialog = (RadWindow)View;
            dialog.Close();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedSite):
                    NotifyOfPropertyChange(nameof(CanDeleteSite));
                    break;
            }
        }
    }
}
