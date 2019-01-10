using System;
using System.IO;
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
            PluginFloder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PLUGIN_DIRECTORY);
        }
    }
}
