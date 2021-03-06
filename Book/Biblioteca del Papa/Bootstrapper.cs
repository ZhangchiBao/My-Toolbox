using Biblioteca_del_Papa.Builders;
using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Biblioteca_del_Papa.Pages;
using Stylet;
using StyletIoC;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Biblioteca_del_Papa
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe").Select(a => Assembly.LoadFile(a)).ToArray();
            builder.Bind<ITabItem>().ToAllImplementations(assemblies, false).InSingletonScope();
            //builder.Bind<ShellViewModel>().ToSelf().InSingletonScope();
            //builder.Bind<ShellView>().ToSelf().InSingletonScope();
            builder.Bind<ContentControl>().ToAllImplementations(assemblies, false).InSingletonScope();
            builder.Bind<BookView>().ToSelf().InSingletonScope();
            builder.Bind<BookViewModel>().ToSelf().InSingletonScope();
            builder.Bind<IFinder>().ToAllImplementations(assemblies, false);
            builder.Bind<IBuilder>().ToAllImplementations(assemblies, false);
            builder.Bind<DBContext>().ToSelf();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
            #region 自动合并数据库修改
            try
            {
                //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Biblioteca_del_Papa.Migrations.Configuration>());
            }
            catch
            {
            }
            #endregion
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
