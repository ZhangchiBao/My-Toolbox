using Biblioteca_del_Papa.Builders;
using Biblioteca_del_Papa.Entities;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.Pages
{
    public class BuildEbookViewModel : Screen
    {
        private readonly IContainer container;

        public BuildEbookViewModel(IContainer container)
        {
            this.container = container;
            Builders = new ObservableCollection<IBuilder>(container.GetAll<IBuilder>());
            SelectedBuilder = Builders.FirstOrDefault();
        }

        public BookShowEntity Book { get; set; }

        public ObservableCollection<IBuilder> Builders { get; }

        public IBuilder SelectedBuilder { get; set; }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            RequestClose(false);
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            SelectedBuilder.Builde(Book.ID);
            RequestClose(true);
        }
    }
}
