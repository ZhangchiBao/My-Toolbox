using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookReading.Entities;
using BookReading.Libs;
using Stylet;
using StyletIoC;

namespace BookReading.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(IContainer container, IWindowManager windowManager, IViewManager viewManager) : base(container, windowManager, viewManager)
        {
            FinderCollection = new ObservableCollection<FinderStatusModel>(container.GetAll<IFinder>().Select(finder => new FinderStatusModel(finder)));
        }

        #region 公开属性
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 能否执行搜索
        /// </summary>
        public bool CanDoSearch => !string.IsNullOrWhiteSpace(Keyword);

        /// <summary>
        /// 能否执行下载
        /// </summary>
        public bool CanDownload => false;

        public ObservableCollection<FinderStatusModel> FinderCollection { get; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 执行搜索
        /// </summary>
        public void DoSearch()
        {
            foreach (var finder in FinderCollection)
            {
                Task.Run(() =>
                {
                    ExecuteOnView(() =>
                    {
                        finder.DoneStatus = DoneStatus.Doing;
                        var result = finder.Finder.SearchByKeyword(Keyword);
                        Task.Delay(3000).ContinueWith(task =>
                        {
                            ExecuteOnView(() =>
                            {
                                finder.DoneStatus = DoneStatus.Done;
                            });
                        });
                    });
                });
            }
        }

        /// <summary>
        /// 执行退出
        /// </summary>
        public void Exist()
        {
            base.RequestClose();
        }

        /// <summary>
        /// 执行下载
        /// </summary>
        public void Download()
        {

        }
        #endregion
    }
}
