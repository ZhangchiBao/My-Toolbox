using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookReading.Libs;
using Stylet;

namespace BookReading.Entities
{
    public class FinderStatusModel : PropertyChangedBase
    {
        public FinderStatusModel(IFinder finder)
        {
            this.Finder = finder;
        }

        public IFinder Finder { get; }

        public DoneStatus DoneStatus { get; set; }
    }

    public enum DoneStatus
    {
        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NoStart,

        /// <summary>
        /// 未开始
        /// </summary>
        [Description("进行中")]
        Doing,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Done
    }
}
