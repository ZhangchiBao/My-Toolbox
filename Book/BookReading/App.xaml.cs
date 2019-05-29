using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace BookReading
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用目录
        /// </summary>
        public static string APP_FLODER { get; private set; }

        /// <summary>
        /// 书架目录
        /// </summary>
        public static string BOOKSHELF_FLODER { get; private set; }

        public App()
        {
            #region 初始化应用目录
            var documentFloder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var appName = typeof(App).Assembly.GetName().Name;

            APP_FLODER = Path.Combine(documentFloder, appName);
            if (!Directory.Exists(APP_FLODER))
            {
                Directory.CreateDirectory(APP_FLODER);
            }

            BOOKSHELF_FLODER = Path.Combine(APP_FLODER, "shelf");
            if (!Directory.Exists(BOOKSHELF_FLODER))
            {
                Directory.CreateDirectory(BOOKSHELF_FLODER);
            }

            #endregion
            InitializeCefSharp();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            var settings = new CefSettings();
            settings.Locale = "zh-CN";
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }
    }
}
