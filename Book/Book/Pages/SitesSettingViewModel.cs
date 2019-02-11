using Book.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

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
            SiteDetailViewModel = container.Get<SiteDetailViewModel>();
        }

        public ObservableCollection<SiteInfo> Sites { get; set; } = new ObservableCollection<SiteInfo>();

        public SiteInfo SelectedSite { get; set; }

        public SiteDetailViewModel SiteDetailViewModel { get; set; }

        public bool CanDeleteSite => SelectedSite != null;

        public async void AddSite()
        {
            var site = new SiteInfo();
            var siteDetailViewModel = container.Get<SiteDetailViewModel>();
            siteDetailViewModel.Site = site;
            var siteDetailView = (CustomDialog)viewManager.CreateAndBindViewForModelIfNecessary(siteDetailViewModel);
            await ((MetroWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>())).ShowMetroDialogAsync(siteDetailView);
        }

        public void DeleteSite()
        {
            Sites.Remove(SelectedSite);
        }

        public async void SitesGridDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)e.OriginalSource).DataContext is SiteInfo site)
            {
                var siteDetailViewModel = container.Get<SiteDetailViewModel>();
                siteDetailViewModel.Site = site;
                var siteDetailView = (CustomDialog)viewManager.CreateAndBindViewForModelIfNecessary(siteDetailViewModel);
                await ((MetroWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>())).ShowMetroDialogAsync(siteDetailView);
            }
        }

        public async void CloseWindow()
        {
            var dialog = (CustomDialog)View;
            await ((MetroWindow)viewManager.CreateAndBindViewForModelIfNecessary(container.Get<ShellViewModel>())).HideMetroDialogAsync(dialog);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SelectedSite):
                    SiteDetailViewModel.Site = SelectedSite;
                    NotifyOfPropertyChange(nameof(SiteDetailViewModel));
                    NotifyOfPropertyChange(nameof(CanDeleteSite));
                    break;
            }
        }
    }
}
