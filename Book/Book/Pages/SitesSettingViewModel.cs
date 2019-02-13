﻿using AutoMapper;
using Book.Models;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class SitesSettingViewModel : Screen
    {
        private readonly IViewManager viewManager;
        private readonly IContainer container;
        private readonly DBContext db;

        public SitesSettingViewModel(IViewManager viewManager, IContainer container, DBContext db)
        {
            this.viewManager = viewManager;
            this.container = container;
            this.db = db;
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
            container.Get<SiteDetailViewModel>().Site = site;
            var siteDetailView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<SiteDetailViewModel>());
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
                container.Get<SiteDetailViewModel>().Site = site;
                var siteDetailView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<SiteDetailViewModel>());
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

        public void LoadSites()
        {
            Sites = new ObservableCollection<SiteInfo>(db.Sites.Select(a => Mapper.Map<SiteInfo>(a)));
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
