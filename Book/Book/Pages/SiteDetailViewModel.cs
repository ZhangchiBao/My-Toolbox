using AutoMapper;
using Book.Models;
using Newtonsoft.Json;
using Stylet;
using StyletIoC;
using System.Linq;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class SiteDetailViewModel : Screen
    {
        private readonly IContainer container;
        private readonly IViewManager viewManager;
        private readonly SitesDBContext db;

        public SiteDetailViewModel(IContainer container, IViewManager viewManager, SitesDBContext db)
        {
            this.container = container;
            this.viewManager = viewManager;
            this.db = db;
        }

        public SiteInfo Site { get; set; }

        public SiteInfo ShowSite { get; set; }

        public bool CanSaveChange => ShowSite != null && !string.IsNullOrEmpty(ShowSite.Name);

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Site):
                    ObjToProperties();
                    break;
                case "Name":
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
            if (Site.ID == 0)
            {
                db.Sites.Add(Mapper.Map<TB_Site>(Site));
            }
            else
            {
                var site = db.Sites.Single(a => a.ID == Site.ID);
                site.Update(Mapper.Map<TB_Site>(Site), nameof(site.ID));
            }
            db.SaveChanges();
            container.Get<SitesSettingViewModel>().LoadSites();
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
            Site = null;
        }

        private void ObjToProperties()
        {
            ShowSite = JsonConvert.DeserializeObject<SiteInfo>(JsonConvert.SerializeObject(Site));
            if (ShowSite != null)
            {
                ShowSite.PropertyChanged += (s, e) =>
                {
                    OnPropertyChanged(e.PropertyName);
                };
            }

            NotifyOfPropertyChange(nameof(CanSaveChange));
        }

        private void PropertiesToObj()
        {
            Site = JsonConvert.DeserializeObject<SiteInfo>(JsonConvert.SerializeObject(ShowSite));
        }
    }
}
