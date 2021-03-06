using BookReading.Libs;
using BookReading.ViewModels;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BookReading
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            builder.Bind<BookContext>().ToSelf().InSingletonScope();
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var dllFile in Directory.GetFiles(System.Environment.CurrentDirectory, "*.dll"))
            {
                try
                {
                    assemblies.Add(Assembly.LoadFile(dllFile));
                }
                catch
                {
                }
            }
            builder.Bind<IFinder>().ToAllImplementations(assemblies);
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
