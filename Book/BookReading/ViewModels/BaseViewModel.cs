using Stylet;
using StyletIoC;
using System;

namespace BookReading.ViewModels
{
    public class BaseViewModel : Screen
    {
        /// <summary>
        /// IOC容器
        /// </summary>
        protected readonly IContainer container;

        /// <summary>
        /// IWindowManager
        /// </summary>
        protected readonly IWindowManager windowManager;

        /// <summary>
        /// IViewManager
        /// </summary>
        protected readonly IViewManager viewManager;

        /// <summary>
        /// 初始化一个ViewModel基类
        /// </summary>
        /// <param name="container">IContainer</param>
        /// <param name="windowManager">IWindowManager</param>
        /// <param name="viewManager">IViewManager</param>
        public BaseViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager)
        {
            this.container = container;
            this.windowManager = windowManager;
            this.viewManager = viewManager;
        }

        /// <summary>
        /// 在视图线程上执行方法
        /// </summary>
        /// <param name="callback"></param>
        protected void ExecuteOnView(Action callback)
        {
            View.Dispatcher.Invoke(callback);
        }
    }
}
