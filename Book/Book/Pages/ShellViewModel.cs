using Book.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace Book.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesSettingViewModel sitesSettingViewModel;
        private MetroWindow view;

        public ShellViewModel(IViewManager viewManager, IContainer container)
        {
            this.container = container;
            this.viewManager = viewManager;
            sitesSettingViewModel = container.Get<SitesSettingViewModel>();
            MenuItemCommand = new Command<Action>(a => a?.Invoke());
            DropdownMenus = new Dictionary<string, Action>();
            DropdownMenus.Add("网站设置", SitesSetting);
            LoadSites();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            view = (MetroWindow)View;
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        private async void SitesSetting()
        {
            LoadSites();
            var sitesSettingView = (BaseMetroDialog)viewManager.CreateAndBindViewForModelIfNecessary(sitesSettingViewModel);
            await view.ShowMetroDialogAsync(sitesSettingView);
        }

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
        /// 下拉菜单
        /// </summary>
        public Dictionary<string, Action> DropdownMenus { get; set; }

        /// <summary>
        /// 菜单操作命令
        /// </summary>
        public ICommand MenuItemCommand { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchKeyword { get; set; }

        /// <summary>
        /// 搜索输入框按键被按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SearchBoxKeydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(SearchKeyword))
            {
                var list = LocalSearch();
                if (list == null)
                {
                    var searchViewModel = container.Get<SearchViewModel>();
                    searchViewModel.SearchKeyword = SearchKeyword;
                    var searchView = (BaseMetroDialog)viewManager.CreateAndBindViewForModelIfNecessary(searchViewModel);
                    //await window.ShowMessageAsync("This is the title", "Some message");
                    await view.ShowMetroDialogAsync(searchView);
                    searchViewModel.DoSearch();
                }
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
            }
        }
    }
}
