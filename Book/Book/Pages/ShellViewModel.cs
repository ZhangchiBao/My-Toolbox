using Book.Models;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesSettingViewModel sitesSettingViewModel;

        public ShellViewModel(IViewManager viewManager, IContainer container)
        {
            this.container = container;
            this.viewManager = viewManager;
            sitesSettingViewModel = container.Get<SitesSettingViewModel>();
            MenuItemCommand = new Command<Action>(a => a?.Invoke());
            LoadSites();
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        public void SitesSetting()
        {
            LoadSites();
            var sitesSettingView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(sitesSettingViewModel);
            sitesSettingView.Owner = (Window)View;
            sitesSettingView.ShowDialog();
        }

        /// <summary>
        /// 加载站点
        /// </summary>
        private void LoadSites()
        {
            try
            {
                sitesSettingViewModel.Sites = new ObservableCollection<SiteInfo>();
                XElement xElement = XElement.Load("sites.xml");
                var nodes = xElement.Nodes();
                foreach (var xNode in nodes)
                {
                    var json = JsonConvert.SerializeXNode(xNode, Formatting.None, true);
                    sitesSettingViewModel.Sites.Add(JsonConvert.DeserializeObject<SiteInfo>(json));
                }
            }
            catch { }
        }

        /// <summary>
        /// 菜单操作命令
        /// </summary>
        public ICommand MenuItemCommand { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword { get; set; }

        /// <summary>
        /// 能否执行搜索
        /// </summary>
        public bool CanDoSearch => !string.IsNullOrEmpty(SearchKeyword);

        /// <summary>
        /// 搜索输入框按键被按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SearchBoxKeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && CanDoSearch)
            {
                DoSearch();
            }
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        public void DoSearch()
        {
            var list = LocalSearch();
            if (list == null)
            {
                var searchViewModel = container.Get<SearchViewModel>();
                searchViewModel.SearchKeyword = SearchKeyword;
                var searchView = (RadWindow)viewManager.CreateAndBindViewForModelIfNecessary(searchViewModel);
                searchView.Owner = (Window)View;
                searchViewModel.DoSearch();
                searchView.ShowDialog();
            }
        }

        /// <summary>
        /// 搜索本地书架
        /// </summary>
        /// <returns></returns>
        private object LocalSearch()
        {
            return null;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SearchKeyword):
                    NotifyOfPropertyChange(nameof(CanDoSearch));
                    break;
            }
        }
    }
}
