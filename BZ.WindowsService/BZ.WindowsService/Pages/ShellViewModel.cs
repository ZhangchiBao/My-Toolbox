using System;
using System.Collections.Generic;
using BZ.WindowsService.Common;
using BZ.WindowsService.Helper;
using Stylet;
using StyletIoC;
using System.Linq;
using System.Collections.ObjectModel;

namespace BZ.WindowsService.Pages
{
    public class ShellViewModel : Screen
    {
        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer Container;
        private readonly ServiceHelper serviceHelper;
        private readonly ConfigHelper configHelper;

        public string ServiceName { get; set; }

        public bool ServiceIsRunning { get; set; }

        public ObservableCollection<ConfigHelper.PluginSection> Plugins { get; set; }

        public ShellViewModel(IContainer container)
        {
            Container = container;
            serviceHelper = container.Get<ServiceHelper>();
            configHelper = container.Get<ConfigHelper>();
            InitialConfig();
            ServiceName = configHelper.ServiceSetting.ServiceName;
            CheckServicStatus();
        }

        private void InitialConfig()
        {
            var plugins = Container.GetAll<IPlugin>();
            var nameInSetting = configHelper.ServiceSetting.Plugins.GetKeys();
            var newPluginNames = plugins.Select(a => a.PluginData.Name).Except(nameInSetting);
            var delPluginNames = nameInSetting.Except(plugins.Select(a => a.PluginData.Name));

            foreach (var name in newPluginNames)
            {
                var plugin = plugins.Single(a => a.PluginData.Name == name);
                configHelper.ServiceSetting.Plugins.Add(new ConfigHelper.PluginSection
                {
                    Name = plugin.PluginData.Name,
                    Author = plugin.PluginData.Author,
                    Enabled = true,
                    Interval = 1
                });
            }
            foreach (var name in delPluginNames)
            {
                configHelper.ServiceSetting.Plugins.Remove(name);
            }
            configHelper.SaveChange();

            Plugins = new ObservableCollection<ConfigHelper.PluginSection>();
            foreach (ConfigHelper.PluginSection plugin in configHelper.ServiceSetting.Plugins)
            {
                Plugins.Add(plugin);
            }
        }

        public void DoTest(ConfigHelper.PluginSection pluginSection)
        {
            var plugin = Container.GetAll<IPlugin>().SingleOrDefault(a => a.PluginData.Name == pluginSection.Name);
            //if (plugin != null)
            //{
            //    plugin.Test();
            //}
            plugin?.Test();
        }

        private void CheckServicStatus()
        {
            if (string.IsNullOrEmpty(ServiceName))
            {
                ServiceIsRunning = false;
                return;
            }
            ServiceIsRunning = serviceHelper.IsRunning(ServiceName);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

        }
    }
}
