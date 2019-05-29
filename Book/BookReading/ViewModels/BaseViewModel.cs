using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading.ViewModels
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
