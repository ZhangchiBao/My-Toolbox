using Stylet;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SVGReader.Entity
{
    public class FloderEntity : ValidatingModelBase
    {
        public string Path { get; set; }

        public ObservableCollection<FloderEntity> Children { get; set; } = new ObservableCollection<FloderEntity>();

        //protected override void OnPropertyChanged(string propertyName)
        //{
        //    base.OnPropertyChanged(propertyName);
        //    switch (propertyName)
        //    {
        //        case nameof(Path):
        //            Task.Run(() =>
        //            {
        //                try
        //                {
        //                    var directoryList = Directory.GetDirectories(Path);
        //                    Children = new ObservableCollection<FloderEntity>(directoryList.Select(a => new FloderEntity { Path = a }));
        //                }
        //                catch
        //                {
        //                    Children = new ObservableCollection<FloderEntity>();
        //                }
        //            });
        //            break;
        //    }
        //}
    }
}
