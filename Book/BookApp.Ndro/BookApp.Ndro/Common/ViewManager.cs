using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BookApp.Ndro.Common
{
    public class ViewManager
    {
        private static Dictionary<Type, Type> view_viewmodelDictionary = new Dictionary<Type, Type>();

        public static void RegisterView<V, VM>()
            where V : Page
            where VM : BaseViewModel
        {
            RegisterView(typeof(V), typeof(VM));
        }

        public static void RegisterView(Type viewType, Type viewmodelType)
        {
            view_viewmodelDictionary.Add(viewType, viewmodelType);
        }

        public static T CreateView<T>()
            where T : Page
        {
            var viewType = typeof(T);
            if (view_viewmodelDictionary.ContainsKey(viewType))
            {
                var vmType = view_viewmodelDictionary[viewType];
                var view = (T)IOC.Get(viewType);
                var viewModel = (BaseViewModel)IOC.Get(vmType);
                if (viewModel.View == null)
                {
                    viewModel.View = view;
                }
                view.BindingContext = viewModel;
                return view;
            }
            throw new Exception($"视图{viewType.FullName}未注册");
        }
    }
}
