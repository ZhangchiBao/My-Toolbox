using Stylet;
using SVGReader.Entity;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SVGReader.Pages
{
    public class ShellViewModel : Screen
    {
        public ShellViewModel()
        {
            Task.Run(() => LoadAllFloder());
        }

        private void LoadAllFloder()
        {
            var drives = DriveInfo.GetDrives();
            FolderCollection = new ObservableCollection<FloderEntity>(drives.Select(a => new FloderEntity { Path = a.Name }));
        }

        public ObservableCollection<FloderEntity> FolderCollection { get; set; } = new ObservableCollection<FloderEntity>();
    }
}
