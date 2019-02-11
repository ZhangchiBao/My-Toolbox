using Book.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Book.Pages
{
    public class SiteDetailViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;

        public SiteDetailViewModel(IContainer container, IViewManager viewManager)
        {
            this.container = container;
            this.viewManager = viewManager;
        }

        public SiteInfo Site { get; set; }

        /// <summary>
        /// Site name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Site index url
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// url for search book
        /// </summary>
        public string SearchURL { get; set; }

        public uint SearchSize { get; set; }

        /// <summary>
        /// book name node
        /// </summary>
        public string BookNameNode { get; set; }

        public string AuthorNode { get; set; }

        public string BookURLNode { get; set; }

        public bool CanSaveChange => !string.IsNullOrEmpty(Name);

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Site):
                    ObjToProperties();
                    break;
                case nameof(Name):
                    NotifyOfPropertyChange(nameof(CanSaveChange));
                    break;
            }
        }

        public void SaveChange()
        {
            PropertiesToObj();
            SaveSites();
            CloseDialog();
        }

        /// <summary>
        /// Save all sites to local
        /// </summary>
        private void SaveSites()
        {
            var sitesSettingViewModel = container.Get<SitesSettingViewModel>();
            if (Site.ID == Guid.Empty)
            {
                Site.ID = Guid.NewGuid();
                sitesSettingViewModel.Sites.Add(Site);
            }
            else
            {
                var site = sitesSettingViewModel.Sites.Single(a => a.ID == Site.ID);
                site.Update(Site, nameof(site.ID));
            }
            XElement xElement = new XElement("Sites");
            foreach (var site in sitesSettingViewModel.Sites)
            {
                xElement.Add(JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(site), "Site").Root);
            }
            //= JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(sitesSettingViewModel.Sites, new ActionJsonConverter())).Parent;
            xElement.Save("sites.xml");
        }

        /// <summary>
        /// Cancel all change
        /// </summary>
        public void CancelChange()
        {
            ObjToProperties();
            CloseDialog();
        }

        /// <summary>
        /// Close dialog
        /// </summary>
        private async void CloseDialog()
        {
            var dialog = (CustomDialog)View;
            await ((MetroWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>())).HideMetroDialogAsync(dialog);
        }

        private void ObjToProperties()
        {
            Name = Site?.Name;
            URL = Site?.URL;
            BookNameNode = Site?.BookNameNode;
            AuthorNode = Site?.AuthorNode;
            BookURLNode = Site?.BookURLNode;
            SearchURL = Site?.SearchURL;
        }

        private void PropertiesToObj()
        {
            Site.Name = Name;
            Site.URL = URL;
            Site.BookNameNode = BookNameNode;
            Site.BookURLNode = BookURLNode;
            Site.AuthorNode = AuthorNode;
            Site.SearchURL = SearchURL;
        }
    }
}
