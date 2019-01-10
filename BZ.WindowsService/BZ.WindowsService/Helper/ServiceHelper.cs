using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BZ.WindowsService.Helper
{
    public class ServiceHelper
    {
        /// <summary>
        /// 检查服务存在的存在性
        /// </summary>
        /// <param name=" NameService ">服务名</param>
        /// <returns>存在返回 true,否则返回 false;</returns>
        public bool IsServiceIsExisted(string NameService)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController s in services)
            {
                if (s.ServiceName.ToLower() == NameService.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 安装Windows服务
        /// </summary>
        /// <param name="serverName">服务名</param>
        /// <param name="filepath">程序文件路径</param>
        /// <returns></returns>
        public bool InstallService(string serverName, string filepath)
        {
            string[] cmds = new string[]
            {
               $"sc create {serverName} binpath= \"{filepath} -s\" displayname= \"{serverName}\"",
               $"sc config {serverName} start= auto",
               $"net start {serverName}"
            };

            var result = Cmd(cmds);

            if (!string.IsNullOrEmpty(result) && result.IndexOf("服务已经启动成功") > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 卸载Windows服务
        /// </summary>
        /// <param name="filepath">服务名</param>
        public void UnInstallService(string serverName)
        {
            if (IsRunning(serverName))
            {
                StopService(serverName);
            }
            string[] cmds = new string[] {
               $"sc delete {serverName}"
            };

            var result = Cmd(cmds);
        }

        /// <summary>
        /// 检查Windows服务是否在运行
        /// </summary>
        /// <param name="name">程序的服务名</param>
        public bool IsRunning(string name)
        {
            bool IsRun = false;
            try
            {
                if (!IsServiceIsExisted(name))
                {
                    return false;
                }
                ServiceController sc = new ServiceController(name);
                if (sc.Status == ServiceControllerStatus.StartPending || sc.Status == ServiceControllerStatus.Running)
                {
                    IsRun = true;
                }
                sc.Close();
            }
            catch
            {
                IsRun = false;
            }
            return IsRun;
        }

        /// <summary>
        /// 启动Windows服务
        /// </summary>
        /// <param name="name">程序的服务名</param>
        /// <returns>启动成功返回 true,否则返回 false;</returns>
        public bool StartService(string name)
        {
            ServiceController sc = new ServiceController(name);
            if (sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.StopPending)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 100));
            }
            else
            {
            }
            sc.Close();
            return true;
        }

        /// <summary>
        /// 停止Windows服务
        /// </summary>
        /// <param name="name">程序的服务名</param>
        /// <returns>停止成功返回 true,否则返回 false;</returns>
        public bool StopService(string name)
        {
            ServiceController sc = new ServiceController(name);
            if (sc.Status == ServiceControllerStatus.Running || sc.Status == ServiceControllerStatus.StartPending)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));
            }
            else
            {
            }
            sc.Close();
            return true;
        }

        /// <summary>
        /// 重启Windows服务
        /// </summary>
        /// <param name="name">程序的服务名</param>
        /// <returns>重启成功返回 true,否则返回 false;</returns>
        public bool RefreshService(string name)
        {
            return StopService(name) && StartService(name);
        }

        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public string Cmd(string[] cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;
            for (int i = 0; i < cmd.Length; i++)
            {
                p.StandardInput.WriteLine(cmd[i].ToString());
            }
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }
    }
}
