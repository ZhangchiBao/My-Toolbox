﻿using BZ.WindowsService.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPlugin
{
    public class TestPlugin : IPlugin
    {
        public PluginData PluginData
        {
            get
            {
                return new PluginData
                {
                    Name = "TestPlugin",
                    Author = "Zhang"
                };
            }
        }

        public void Start()
        {
            File.AppendAllText("d:\\temp.log", DateTime.Now.ToString() + "\t" + Thread.CurrentThread.ManagedThreadId + Environment.NewLine, Encoding.UTF8);
        }

        public void Stop()
        {

        }

        public void Test()
        {
            Start();
        }
    }
}
