using Book.Models;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System;
using System.Linq;
using System.Xml.Linq;
using Telerik.Windows.Controls;

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
        /// 站点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 站点主页
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 站点搜索链接
        /// </summary>
        public string SearchURL { get; set; }

        /// <summary>
        /// 搜索结果页容量
        /// </summary>
        public uint? SearchSize { get; set; }

        /// <summary>
        /// 书籍搜索结果节点
        /// </summary>
        public string BookResultsNode { get; set; }

        /// <summary>
        /// 书名节点
        /// </summary>
        public string BookNameNode { get; set; }

        /// <summary>
        /// 作者节点
        /// </summary>
        public string AuthorNode { get; set; }

        /// <summary>
        /// 最新章节节点
        /// </summary>
        public string UpdateNode { get; set; }

        /// <summary>
        /// 简介节点
        /// </summary>
        public string DescriptionNode { get; set; }

        /// <summary>
        /// 书地址节点
        /// </summary>
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

        /// <summary>
        /// 保存修改
        /// </summary>
        public void SaveChange()
        {
            PropertiesToObj();
            SaveSites();
            CloseDialog();
        }

        /// <summary>
        /// 保存所有站点信息
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
            xElement.Save("sites.xml");
        }

        /// <summary>
        /// 取消所有修改
        /// </summary>
        public void CancelChange()
        {
            ObjToProperties();
            CloseDialog();
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        private void CloseDialog()
        {
            var dialog = (RadWindow)View;
            dialog.Close();
        }

        private void ObjToProperties()
        {
            Name = Site?.Name;
            URL = Site?.URL;
            BookNameNode = Site?.BookNameNode;
            AuthorNode = Site?.AuthorNode;
            BookURLNode = Site?.BookURLNode;
            SearchURL = Site?.SearchURL;
            SearchSize = Site?.SearchSize;
            BookResultsNode = Site?.BookResultsNode;
            DescriptionNode = Site?.DescriptionNode;
            UpdateNode = Site?.UpdateNode;
        }

        private void PropertiesToObj()
        {
            Site.Name = Name;
            Site.URL = URL;
            Site.BookNameNode = BookNameNode;
            Site.BookURLNode = BookURLNode;
            Site.AuthorNode = AuthorNode;
            Site.SearchURL = SearchURL;
            Site.SearchSize = SearchSize;
            Site.BookResultsNode = BookResultsNode;
            Site.DescriptionNode = DescriptionNode;
            Site.UpdateNode = UpdateNode;
        }
    }
}
