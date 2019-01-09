using System;
using System.Collections.Generic;
using BZ.WindowsService.Common;
using BZ.WindowsService.Helper;
using Stylet;
using StyletIoC;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using Config = BZ.WindowsService.Helper.ConfigHelper;
using BZ.WindowsService.Model;

namespace BZ.WindowsService.Pages
{
    public class ShellViewModel : Screen
    {
        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer Container;
        private readonly ServiceHelper serviceHelper;
        private readonly Config configHelper;

        public string ServiceName { get; set; }

        public bool ServiceIsRunning { get; set; }

        public bool CanSaveSettings { get; set; }

        public ObservableCollection<PluginSettingModel> Plugins { get; set; }

        public ShellViewModel(IContainer container)
        {
            Container = container;
            serviceHelper = container.Get<ServiceHelper>();
            configHelper = container.Get<Config>();
            InitialConfig();
            CheckServicStatus();
        }

        private void InitialConfig()
        {
            ServiceName = configHelper.ServiceSetting.ServiceName;
            var plugins = Container.GetAll<IPlugin>();
            var nameInSetting = configHelper.ServiceSetting.PluginSettings.GetKeys();
            var newPluginNames = plugins.Select(a => a.PluginData.Name).Except(nameInSetting);
            var delPluginNames = nameInSetting.Except(plugins.Select(a => a.PluginData.Name));

            // 添加新插件配置项
            foreach (var name in newPluginNames)
            {
                var plugin = plugins.Single(a => a.PluginData.Name == name);
                configHelper.ServiceSetting.PluginSettings.Add(new Config.PluginSettingSection
                {
                    Name = plugin.PluginData.Name,
                    Author = plugin.PluginData.Author,
                    Enabled = true,
                    Interval = 1
                });
            }
            // 移除失效配置项
            foreach (var name in delPluginNames)
            {
                configHelper.ServiceSetting.PluginSettings.Remove(name);
            }
            configHelper.SaveChange();

            Plugins = new ObservableCollection<PluginSettingModel>();
            foreach (Config.PluginSettingSection plugin in configHelper.ServiceSetting.PluginSettings)
            {
                Plugins.Add(new PluginSettingModel
                {
                    Author = plugin.Author,
                    Enabled = plugin.Enabled,
                    Interval = plugin.Interval,
                    Name = plugin.Name,
                    PropertyChangedAction = () => { OnPropertyChanged(nameof(Plugins)); }
                });
            }
            CanSaveSettings = false;
        }

        public void DoTest(PluginSettingModel pluginSetting)
        {
            var plugin = Container.GetAll<IPlugin>().SingleOrDefault(a => a.PluginData.Name == pluginSetting.Name);
            plugin?.Test();
            MessageBox.Show("测试运行结束");
        }

        public void SaveSetting(PluginSettingModel pluginSetting)
        {
            foreach (Config.PluginSettingSection plugin in configHelper.ServiceSetting.PluginSettings)
            {
                if (plugin.Name == pluginSetting.Name)
                {
                    plugin.Interval = pluginSetting.Interval;
                    plugin.Enabled = pluginSetting.Enabled;
                    break;
                }
            }
            configHelper.SaveChange();
            InitialConfig();
        }

        public void SaveSettings()
        {
            configHelper.ServiceSetting.ServiceName = ServiceName;
            foreach (Config.PluginSettingSection plugin in configHelper.ServiceSetting.PluginSettings)
            {
                var pluginSetting = Plugins.SingleOrDefault(a => a.Name == plugin.Name);
                if (pluginSetting != null)
                {
                    plugin.Interval = pluginSetting.Interval;
                    plugin.Enabled = pluginSetting.Enabled;
                }
            }
            configHelper.SaveChange();
            InitialConfig();
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
            switch (propertyName)
            {
                case nameof(ServiceName):
                    CanSaveSettings = CheckSetting();
                    break;
                case nameof(Plugins):
                    CanSaveSettings = CheckSetting();
                    break;
            }
        }

        private bool CheckSetting()
        {
            var setting = configHelper.ServiceSetting;
            if (setting.ServiceName != ServiceName)
            {
                return true;
            }
            if (Plugins == null)
            {
                return true;
            }
            foreach (Config.PluginSettingSection plugin in setting.PluginSettings)
            {
                if (!Plugins.Any(a => a.Name == plugin.Name && a.Interval == plugin.Interval && a.Author == plugin.Author && a.Enabled == plugin.Enabled))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
