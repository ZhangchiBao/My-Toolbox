using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BZ.WindowsService.Common;
using BZ.WindowsService.Helper;
using BZ.WindowsService.Model;
using Newtonsoft.Json;

namespace BZ.WindowsService
{
    internal partial class BZ_Service : ServiceBase
    {
        public BZ_Service()
        {
            InitializeComponent();
        }

        public IList<IPlugin> Plugins { get; internal set; }
        public ConfigHelper ConfigHelper { get; internal set; }

        private static CancellationTokenSource tokenSource;

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            var serviceSetting = ConfigHelper.ServiceSetting;
            // 读取服务配置
            var pluginSettings = new List<PluginSettingModel>();
            foreach (ConfigHelper.PluginSettingSection settingSection in serviceSetting.PluginSettings)
            {
                pluginSettings.Add(new PluginSettingModel(settingSection));
            }
            pluginSettings.ForEach(setting =>
            {
                var plugin = Plugins.SingleOrDefault(a => a.PluginData.Name == setting.Name);
                if (plugin == null)
                {
                    return;
                }
                for (int i = 0; i < setting.ThreadCount; i++)
                {
                    Task.Run(() =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            plugin.Start();
                            Thread.Sleep(setting.Interval);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                        }
                    });
                }
            });
            //Plugins.ToList().ForEach(a =>
            //{
            //    Task.Run(() =>
            //    {
            //        var setting = pluginSettings.SingleOrDefault(pluginSetting => pluginSetting.Name == a.PluginData.Name && pluginSetting.Enabled);
            //        if (setting != null)
            //        {
            //            while (!cancellationToken.IsCancellationRequested)
            //            {
            //                if (cancellationToken.IsCancellationRequested)
            //                {
            //                    break;
            //                }
            //                a.Start();
            //                Thread.Sleep(setting.Interval);
            //            }
            //        }
            //    });
            //});
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            tokenSource.Cancel();
        }
    }
}
