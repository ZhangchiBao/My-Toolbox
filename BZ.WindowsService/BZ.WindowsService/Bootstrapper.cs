using System;
using Stylet;
using StyletIoC;
using BZ.WindowsService.Pages;
using System.IO;
using System.Linq;
using System.Reflection;
using BZ.WindowsService.Common;
using BZ.WindowsService.Helper;

namespace BZ.WindowsService
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            var assemblies = Directory.GetDirectories(App.PluginFloder).SelectMany(directory => Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly)).Select(dllPath => Assembly.LoadFile(dllPath)).ToArray();
            if (assemblies.Length > 0)
            {
                // 注册所有插件
                builder.Bind<IPlugin>().ToAllImplementations(assemblies).InSingletonScope();
            }
            builder.Bind<ServiceHelper>().ToSelf();
            builder.Bind<ConfigHelper>().ToSelf();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }

        public override void Start(string[] args)
        {
            Initial();
            if (args.Length > 0)
            {
                /*
                *Start Services...
                */
                return;
            }

            base.Start(args);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initial()
        {
            if (!Directory.Exists(App.PluginFloder))
            {
                Directory.CreateDirectory(App.PluginFloder);
            }
        }
    }
}
