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
        private bool runService;

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            var assemblies = Directory.GetFiles(App.PluginFloder, "*.dll", SearchOption.AllDirectories).Select(dllPath => Assembly.LoadFile(dllPath)).ToArray();
            if (assemblies.Length > 0)
            {
                // 注册所有插件
                builder.Bind<IPlugin>().ToAllImplementations(assemblies).InSingletonScope();
            }
            builder.Bind<ServiceHelper>().ToSelf();
            builder.Bind<ConfigHelper>().ToSelf();
            builder.Bind<BZ_Service>().ToSelf();
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }

        public override void Start(string[] args)
        {
            Initial();
            if (args.Length > 0 && args[0] == "-s")
            {
                runService = true;
            }

            base.Start(args);
        }

        protected override void Launch()
        {
            if (runService)
            {
                // Start Services...
                var service = Container.Get<BZ_Service>();
                service.Plugins = Container.GetAll<IPlugin>().ToList();
                service.ConfigHelper = Container.Get<ConfigHelper>();
                //直接运行服务
                System.ServiceProcess.ServiceBase.Run(service);
                return;
            }
            base.Launch();
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
