using Stylet;
using StyletIoC;
using System;
using System.Windows;

namespace BookReading.ViewModels
{
    public delegate void ViewLoadedEventHandler();
    public class BaseViewModel : Screen
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected readonly BookContext db;

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
        public BaseViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager, BookContext db)
        {
            this.db = db;
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

        /// <summary>
        /// 当视图加载完成时
        /// </summary>
        public event ViewLoadedEventHandler ViewLoaded;

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();
            ViewLoaded?.Invoke();
        }

        protected void ShowMessage(string messageBoxText, string caption = "提示", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
    }
}
