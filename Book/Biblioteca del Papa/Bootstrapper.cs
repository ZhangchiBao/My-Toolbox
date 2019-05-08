using Biblioteca_del_Papa.DAL;
using Biblioteca_del_Papa.Entities;
using Biblioteca_del_Papa.Finders;
using Biblioteca_del_Papa.Pages;
using Stylet;
using StyletIoC;
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
            var assemblies = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory, "*.exe").Select(a => Assembly.LoadFile(a)).ToArray();
            builder.Bind<ITabItem>().ToAllImplementations(assemblies, false).InSingletonScope();
            builder.Bind<ContentControl>().ToAllImplementations(assemblies, false).InSingletonScope();
            builder.Bind<IFinder>().ToAllImplementations(assemblies, false);
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
            var finders = Container.GetAll<IFinder>();
            using (var db = new DBContext())
            {
                db.Database.CreateIfNotExists();
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
    }
}
