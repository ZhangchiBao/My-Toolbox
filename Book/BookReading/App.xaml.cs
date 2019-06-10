using System;
using System.IO;
using System.Runtime.CompilerServices;
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

            //BOOKSHELF_FLODER = Path.Combine(APP_FLODER, "shelf");
            //if (!Directory.Exists(BOOKSHELF_FLODER))
            //{
            //    Directory.CreateDirectory(BOOKSHELF_FLODER);
            //}

            //string styleCssFile = Path.Combine(BOOKSHELF_FLODER, "style.css");
            //if (!File.Exists(styleCssFile))
            //{
            //    File.WriteAllText(styleCssFile, BookReading.Properties.Resources.style);
            //}

            //string headJsFile = Path.Combine(BOOKSHELF_FLODER, "head.js");
            //if (!File.Exists(headJsFile))
            //{
            //    File.WriteAllText(headJsFile, BookReading.Properties.Resources.head);
            //}
            #endregion
        }
    }
}
