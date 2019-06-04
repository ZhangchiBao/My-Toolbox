using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookReading.MenuHandlers
{
    public class MenuHandler : IContextMenuHandler
    {
        public Dictionary<string, Action> MenuItems = new Dictionary<string, Action>();

        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {

        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return true;
        }

        public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
            //隐藏菜单栏
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            webBrowser.Dispatcher.Invoke(() =>
            {
                webBrowser.ContextMenu = null;
            });
        }

        public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            //绘制了一遍菜单栏  所以初始化的时候不必绘制菜单栏，再此处绘制即可
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            webBrowser.Dispatcher.Invoke(() =>
            {
                var menu = new ContextMenu
                {
                    IsOpen = true
                };

                RoutedEventHandler handler = null;

                handler = (s, e) =>
                {
                    menu.Closed -= handler;

                    //If the callback has been disposed then it's already been executed
                    //so don't call Cancel
                    if (!callback.IsDisposed)
                    {
                        callback.Cancel();
                    }
                };

                menu.Closed += handler;

                if (MenuItems != null && MenuItems.Count > 0)
                {
                    foreach (var item in MenuItems)
                    {
                        menu.Items.Add(new MenuItem { Header = item.Key });
                    }
                }
                //menu.Items.Add(new MenuItem
                //{
                //    Header = "最小化",
                //    //Command = new CustomCommand(MinWindow)
                //});
                //menu.Items.Add(new MenuItem
                //{
                //    Header = "关闭",
                //    //Command = new CustomCommand(CloseWindow)
                //});
                webBrowser.ContextMenu = menu;

            });

            return true;
        }
    }
}
