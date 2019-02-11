using Book.Pages;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Book
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        private readonly List<Type> ExcludeTypes;

        public Bootstrapper()
        {
            ExcludeTypes = new List<Type>();
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            var types = Assembly.GetExecutingAssembly().ExportedTypes;
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
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
