using BZ.WindowsService.Helper;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

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
}
