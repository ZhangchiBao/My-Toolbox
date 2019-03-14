using Stylet;
using StyletIoC;
using SVGReader.Entity;
using SVGReader.Pages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SVGReader
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnLaunch()
        {
            base.OnLaunch();
        }

        private ObservableCollection<FloderEntity> GetChildren(string path, List<FileInfo> directoryInfo)
        {
            return new ObservableCollection<FloderEntity>(directoryInfo.Where(a => a.DirectoryName == path).Select(a => new FloderEntity
            {
                Path = a.FullName,
                Children = GetChildren(a.FullName, directoryInfo)
            }));
        }
    }
}
