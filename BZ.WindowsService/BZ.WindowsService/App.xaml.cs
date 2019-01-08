using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BZ.WindowsService
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 插件目录
        /// </summary>
        public const string PLUGIN_DIRECTORY = "plugins";

        public static string PluginFloder { get; }

        static App()
        {
            PluginFloder = Path.Combine(AppContext.BaseDirectory, PLUGIN_DIRECTORY);
        }
    }
}
