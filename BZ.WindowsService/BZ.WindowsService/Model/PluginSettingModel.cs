using BZ.WindowsService.Helper;
using Stylet;
using System;

namespace BZ.WindowsService.Model
{
    public class PluginSettingModel : PropertyChangedBase
    {
        public PluginSettingModel(ConfigHelper.PluginSettingSection settingSection)
        {
            Name = settingSection.Name;
            Author = settingSection.Author;
            Interval = settingSection.Interval < 0 ? 0 : settingSection.Interval;
            Enabled = settingSection.Enabled;
            ThreadCount = settingSection.ThreadCount <= 0 ? 1 : settingSection.ThreadCount;
        }

        public PluginSettingModel(ConfigHelper.PluginSettingSection settingSection, Common.IPlugin plugin) : this(settingSection)
        {
            Version = new PluginVersionModel { Local = plugin.PluginData.Version };
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public PluginVersionModel Version { get; set; }

        /// <summary>
        /// 运行间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 线程数
        /// </summary>
        public int ThreadCount { get; set; }

        public Action PropertyChangedAction { get; set; }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            PropertyChangedAction?.Invoke();
        }
    }

    /// <summary>
    /// 插件版本
    /// </summary>
    public class PluginVersionModel
    {
        /// <summary>
        /// 本地
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// 远程
        /// </summary>
        public string Remote { get; set; }
    }
}
