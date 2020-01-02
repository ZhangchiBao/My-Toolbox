using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSearcher.UILogic.ViewModels
{
    public class BaseViewModel : Screen
    {
        protected readonly IContainer container;

        public BaseViewModel(IContainer container)
        {
            this.container = container;
        }
    }
}
