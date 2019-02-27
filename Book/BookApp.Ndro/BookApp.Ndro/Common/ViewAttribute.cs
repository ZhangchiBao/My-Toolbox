using System;

namespace BookApp.Ndro.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewAttribute : Attribute
    {
        public ViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }

        public Type ViewType { get; private set; }
    }
}
