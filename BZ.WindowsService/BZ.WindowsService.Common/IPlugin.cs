using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZ.WindowsService.Common
{
    /// <summary>
    /// 插件基接口
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// 插件资料
        /// </summary>
        PluginData PluginData { get; }

        /// <summary>
        /// 开始
        /// </summary>
        void Start();

        /// <summary>
        /// 结束
        /// </summary>
        void Stop();

        /// <summary>
        /// 测试
        /// </summary>
        void Test();
    }
}
