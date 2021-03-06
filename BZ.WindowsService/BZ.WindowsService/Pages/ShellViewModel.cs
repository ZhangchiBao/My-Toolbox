using BZ.WindowsService.Common;
using BZ.WindowsService.Helper;
using BZ.WindowsService.Model;
using Stylet;
using StyletIoC;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Config = BZ.WindowsService.Helper.ConfigHelper;

namespace BZ.WindowsService.Pages
{
    public class ShellViewModel : Screen
    {
        #region 私有字段
        /// <summary>
        /// 容器
        /// </summary>
        private readonly IContainer Container;

        /// <summary>
        /// 服务操作类
        /// </summary>
        private readonly ServiceHelper serviceHelper;

        /// <summary>
        /// 配置文件操作类
        /// </summary>
        private readonly Config configHelper;
        #endregion

        #region 公共属性
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务是否在运行中
        /// </summary>
        public bool ServiceIsRunning { get; set; }

        /// <summary>
        /// 服务是否存在
        /// </summary>
        public bool ServiceExist { get; set; }

        /// <summary>
        /// 能否保存设置
        /// </summary>
        public bool CanSaveSettings { get; set; }

        /// <summary>
        /// 能否启动服务
        /// </summary>
        public bool CanStartService { get; set; }

        /// <summary>
        /// 能否停止服务
        /// </summary>
        public bool CanStopService { get; set; }

        /// <summary>
        /// 能否重新启动服务
        /// </summary>
        public bool CanRestartService { get; set; }

        /// <summary>
        /// 能否安装服务
        /// </summary>
        public bool CanInstallService { get; set; }

        /// <summary>
        /// 能否卸载服务
        /// </summary>
        public bool CanUninstallService { get; set; }

        /// <summary>
        /// 插件清单
        /// </summary>
        public ObservableCollection<PluginSettingModel> Plugins { get; set; }
        #endregion

        public ShellViewModel(IContainer container)
        {
            Container = container;
            serviceHelper = container.Get<ServiceHelper>();
            configHelper = container.Get<Config>();
            InitialConfig();
            CheckServicStatus();
        }

        #region 公共方法
        /// <summary>
        /// 执行测试方法
        /// </summary>
        /// <param name="pluginSetting"></param>
        public void DoTest(PluginSettingModel pluginSetting)
        {
            var plugin = Container.GetAll<IPlugin>().SingleOrDefault(a => a.PluginData.Name == pluginSetting.Name);
            plugin?.Test();
            MessageBox.Show("测试运行结束");
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="pluginSetting"></param>
        public void SaveSetting(PluginSettingModel pluginSetting)
        {
            foreach (Config.PluginSettingSection settingSection in configHelper.ServiceSetting.PluginSettings)
            {
                if (settingSection.Name == pluginSetting.Name)
                {
                    settingSection.Interval = pluginSetting.Interval;
                    settingSection.Enabled = pluginSetting.Enabled;
                    settingSection.ThreadCount = pluginSetting.ThreadCount;
                    break;
                }
            }
            configHelper.SaveChange();
            InitialConfig();
        }

        /// <summary>
        /// 保存全部设置
        /// </summary>
        public void SaveSettings()
        {
            configHelper.ServiceSetting.ServiceName = ServiceName;
            foreach (Config.PluginSettingSection settingSection in configHelper.ServiceSetting.PluginSettings)
            {
                var pluginSetting = Plugins.SingleOrDefault(a => a.Name == settingSection.Name);
                if (pluginSetting != null)
                {
                    settingSection.Interval = pluginSetting.Interval;
                    settingSection.Enabled = pluginSetting.Enabled;
                    settingSection.ThreadCount = pluginSetting.ThreadCount;
                }
            }
            configHelper.SaveChange();
            InitialConfig();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public async void StartService()
        {
            await Task.Run(() =>
            {
                serviceHelper.StartService(ServiceName);
            });
            CheckServicStatus();
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public async void StopService()
        {
            await Task.Run(() =>
            {
                serviceHelper.StopService(ServiceName);
            });
            CheckServicStatus();
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        public async void RestartService()
        {
            await Task.Run(() =>
            {
                serviceHelper.RefreshService(ServiceName);
            });
            CheckServicStatus();
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        public async void InstallService()
        {
            await Task.Run(() =>
            {
                serviceHelper.InstallService(ServiceName, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            });
            CheckServicStatus();
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        public async void UninstallService()
        {
            await Task.Run(() =>
            {
                serviceHelper.UnInstallService(ServiceName);
            });
            CheckServicStatus();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 服务状态检测
        /// </summary>
        private void CheckServicStatus()
        {
            if (string.IsNullOrEmpty(ServiceName))
            {
                ServiceIsRunning = false;
                CanStopService = false;
                CanStartService = false;
                CanInstallService = false;
                CanUninstallService = false;
                return;
            }
            ServiceExist = serviceHelper.IsServiceIsExisted(ServiceName);
            ServiceIsRunning = serviceHelper.IsRunning(ServiceName);
            if (ServiceExist)
            {
                CanUninstallService = true;
                CanInstallService = false;
                if (ServiceIsRunning)
                {
                    CanStopService = true;
                    CanStartService = false;
                }
                else
                {
                    CanStartService = true;
                    CanStopService = false;
                }
                CanRestartService = true;
            }
            else
            {
                CanInstallService = true;
                CanUninstallService = false;
                CanStartService = false;
                CanStopService = false;
                CanRestartService = false;
            }
        }

        /// <summary>
        /// 初始化配置文件
        /// </summary>
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
            foreach (Config.PluginSettingSection pluginSetting in configHelper.ServiceSetting.PluginSettings)
            {
                var plugin = plugins.Single(a => a.PluginData.Name == pluginSetting.Name);
                Plugins.Add(new PluginSettingModel(pluginSetting, plugin)
                {
                    PropertyChangedAction = () => { OnPropertyChanged(nameof(Plugins)); }
                });
            }
            CanSaveSettings = false;
            CheckServicStatus();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(ServiceName):
                    CanSaveSettings = CheckSetting();
                    if (string.IsNullOrWhiteSpace(ServiceName))
                    {
                        CanInstallService = false;
                    }
                    break;
                case nameof(Plugins):
                    CanSaveSettings = CheckSetting();
                    break;
                case nameof(ServiceIsRunning):
                case nameof(ServiceExist):

                    break;
            }
        }

        /// <summary>
        /// 检查配置
        /// </summary>
        /// <returns></returns>
        private bool CheckSetting()
        {
            if (string.IsNullOrEmpty(ServiceName))
            {
                return false;
            }
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
                if (!Plugins.Any(a => a.Name == plugin.Name && a.Interval == plugin.Interval && a.Author == plugin.Author && a.Enabled == plugin.Enabled && a.ThreadCount == plugin.ThreadCount))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
