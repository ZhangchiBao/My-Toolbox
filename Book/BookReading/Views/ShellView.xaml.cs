﻿using BookReading.BrowserHandlers;
using CefSharp;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookReading.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView
    {
        public ShellView(IContainer container)
        {
            InitializeComponent();
            WebBrowser.RegisterAsyncJsObject("wpfObj", container.Get<CallbackObjectForJs>(), new BindingOptions { CamelCaseJavascriptNames = false });
        }
    }
}
