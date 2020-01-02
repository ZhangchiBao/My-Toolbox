using ResourceSearcher.UILogic.Searchers;
using ResourceSearcher.UILogic.ViewModels;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSearcher.UILogic
{
    public class Bootstrapper : Bootstrapper<ShellPageViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<IViewManager>().To<ViewManager>();
            builder.Assemblies.Add(Assembly.GetEntryAssembly());
            builder.Bind<ISearcher>().ToAllImplementations();
            base.ConfigureIoC(builder);
        }

        protected override void DefaultConfigureIoC(StyletIoCBuilder builder)
        {// Mark these as weak-bindings, so the user can replace them if they want
            var viewManagerConfig = new ViewManagerConfig()
            {
                ViewFactory = this.GetInstance,
                ViewAssemblies = builder.Assemblies//new List<Assembly>() { this.GetType().Assembly }
            };
            builder.Bind<ViewManagerConfig>().ToInstance(viewManagerConfig).AsWeakBinding();

            // Bind it to both IViewManager and to itself, so that people can get it with Container.Get<ViewManager>()
            builder.Bind<IViewManager>().And<ViewManager>().To<ViewManager>().AsWeakBinding();

            builder.Bind<IWindowManagerConfig>().ToInstance(this).DisposeWithContainer(false).AsWeakBinding();
            builder.Bind<IWindowManager>().To<WindowManager>().InSingletonScope().AsWeakBinding();
            builder.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope().AsWeakBinding();
            builder.Bind<IMessageBoxViewModel>().To<MessageBoxViewModel>().AsWeakBinding();
            // Stylet's assembly isn't added to the container, so add this explicitly
            builder.Bind<MessageBoxView>().ToSelf();

            builder.Autobind();
        }
    }
}
