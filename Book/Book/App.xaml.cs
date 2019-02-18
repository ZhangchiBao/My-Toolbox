using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Telerik.Windows.Controls;

namespace Book
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 书架目录名
        /// </summary>
        public const string SHELF_DIRECTORY = "shelf";
        private readonly string baseDirectory;

        public App()
        {
            LocalizationManager.Manager = new ChineseLocalizationManager();
            baseDirectory = Path.Combine(Environment.CurrentDirectory, "bin");
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
            var dlls = Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            foreach (var dll in dlls)
            {
                if (File.Exists(Path.Combine(baseDirectory, new FileInfo(dll).Name)))
                {
                    File.Delete(Path.Combine(baseDirectory, new FileInfo(dll).Name));
                }
                File.Move(dll, Path.Combine(baseDirectory, new FileInfo(dll).Name));
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            return Assembly.LoadFrom(baseDirectory);
        }
    }
}
