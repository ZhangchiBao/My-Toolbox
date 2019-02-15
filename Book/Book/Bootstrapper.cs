using AutoMapper;
using Book.ExportMethods;
using Book.Models;
using Book.Pages;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace Book
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        private RadDesktopAlertManager Manager;

        private readonly List<Type> ExcludeTypes;

        public Bootstrapper()
        {
            ExcludeTypes = new List<Type>();
            AlertPositioning();
            Mapper.Initialize(mce =>
            {
                mce.CreateMap<TB_Site, SiteInfo>();
                mce.CreateMap<TB_Book, BookInfo>();
                mce.CreateMap<TB_Chapter, ChapterInfo>();
                mce.CreateMap<BookSearchResult, TB_Book>()
                .ForMember(a => a.Name, opt => opt.MapFrom(b => b.BookName))
                .ForMember(a => a.Type, opt => opt.MapFrom(b => string.Empty))
                .ForMember(a => a.CurrentSource, opt => opt.MapFrom(b => b.SRC))
                .ForMember(a => a.CurrentSiteID, opt => opt.MapFrom(b => b.SiteID))
                ;
            });
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here

            Assembly assembly = Assembly.GetExecutingAssembly();
            var types = assembly.ExportedTypes;
            foreach (var type in types)
            {
                if (type.FullName.StartsWith("Book.Pages"))
                {
                    if (ExcludeTypes.Contains(type))
                    {
                        continue;
                    }
                    builder.Bind(type).ToSelf().InSingletonScope();
                }
            }
            builder.Bind<IExport>().ToAllImplementations(new Assembly[] { assembly }).InSingletonScope();
            builder.Bind<SitesDBContext>().ToSelf().InSingletonScope();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }

        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            //base.OnUnhandledException(e);
            e.Handled = true;
            Exception exception = e.Exception;
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            Manager.ShowAlert(CreateAlert("出现异常", exception.Message));
        }

        private RadDesktopAlert CreateAlert(string header, string content)
        {
            var alert = new RadDesktopAlert();
            alert.Header = header;
            alert.Content = content;
            alert.ShowDuration = 2500;
            return alert;
        }

        private void AlertPositioning()
        {
            if (Manager != null)
            {
                this.Manager.CloseAllAlerts();
            }
            this.Manager = new RadDesktopAlertManager(AlertScreenPosition.BottomRight);
        }
    }
}
