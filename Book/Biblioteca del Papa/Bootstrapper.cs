using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Biblioteca_del_Papa.Migrations;
using Biblioteca_del_Papa.Pages;
using Stylet;
using StyletIoC;
using System;
using System.Data.Entity;
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
            builder.Bind<DBContext>().ToSelf();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
            try
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Configuration>());
            }
            catch
            {
            }
            var finders = Container.GetAll<IFinder>();
            using (var db = Container.Get<DBContext>())
            {
                foreach (var finder in finders)
                {
                    if (db.Finders.All(a => a.Key != finder.FinderKey))
                    {
                        db.Finders.Add(new Finder { Key = finder.FinderKey, Name = finder.FinderName });
                    }
                }
                db.SaveChanges();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            var dbFloder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dbFloder = Path.Combine(dbFloder, "Biblioteca del Papa");
            if (!Directory.Exists(dbFloder))
            {
                Directory.CreateDirectory(dbFloder);
            }
        }
    }
}
