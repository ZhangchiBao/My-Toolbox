using Book.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml.Linq;

namespace Book.Pages
{
    public class ShellViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private MetroWindow view;

        public ShellViewModel(IViewManager viewManager, IContainer container)
        {
            this.container = container;
            this.viewManager = viewManager;
            MenuItemCommand = new Command<Action>(MenuItemClick);
            DropdownMenus = new Dictionary<string, Action>();
            DropdownMenus.Add("网站设置", SitesSetting);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            view = (MetroWindow)View;
        }

        private void MenuItemClick(Action obj)
        {
            obj.Invoke();
        }

        private async void SitesSetting()
        {

            SitesSettingViewModel sitesSettingViewModel = container.Get<SitesSettingViewModel>();
            sitesSettingViewModel.Sites = new System.Collections.ObjectModel.ObservableCollection<SiteInfo>();
            try
            {
                XElement xElement = XElement.Load("sites.xml");
                var nodes = xElement.Nodes();
                foreach (var xNode in nodes)
                {
                    var json = JsonConvert.SerializeXNode(xNode, Formatting.None, true);
                    sitesSettingViewModel.Sites.Add(JsonConvert.DeserializeObject<SiteInfo>(json));
                }
            }
            catch { }
            finally
            {
                var sitesSettingView = (BaseMetroDialog)viewManager.CreateAndBindViewForModelIfNecessary(sitesSettingViewModel);
                await view.ShowMetroDialogAsync(sitesSettingView);
            }
        }

        public Dictionary<string, Action> DropdownMenus { get; set; }

        public ICommand MenuItemCommand { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
            }
        }

        public string SearchKeyword { get; set; }

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

        private object LocalSearch()
        {
            return null;
        }
    }
}
