using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookApp.Ndro.Common
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyChangedBase()
        {
            PropertyChanged += PropertyChangedBase_PropertyChanged;
        }

        private void PropertyChangedBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            
        }

        protected void Set<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (field == null || value == null || !field.Equals(value))
            {
                field = value;
                NotifyPropertyChange(propertyName);
            }
        }

        protected void NotifyPropertyChange([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
