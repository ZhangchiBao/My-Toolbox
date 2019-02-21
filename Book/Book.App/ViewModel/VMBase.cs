using GalaSoft.MvvmLight;
using System;

namespace Book.App.ViewModel
{
    public class VMBase : ViewModelBase
    {
        public VMBase()
        {
            PropertyChanged += VMBase_PropertyChanged;
        }

        private void VMBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            OnPropertyChanged(e.PropertyName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            
        }
    }
}
