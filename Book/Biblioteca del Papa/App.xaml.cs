using System;
using System.IO;
using System.Windows;

namespace Biblioteca_del_Papa
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用目录
        /// </summary>
        public static string APPFloder { get; private set; }

        public App()
        {
            #region 创建数据库目录
            var dbFloder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var floderName = typeof(App).Assembly.GetName().Name;
            APPFloder = Path.Combine(dbFloder, floderName);
            if (!Directory.Exists(APPFloder))
            {
                Directory.CreateDirectory(APPFloder);
            }
            #endregion
        }
    }
}
