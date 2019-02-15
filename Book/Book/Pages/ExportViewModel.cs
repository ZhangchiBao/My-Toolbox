﻿using AutoMapper;
using Book.Common;
using Book.ExportMethods;
using Book.Models;
using HtmlAgilityPack;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Controls;

namespace Book.Pages
{
    public class ExportViewModel : Screen
    {
        private readonly IContainer container;
        private readonly SitesDBContext db;

        public ExportViewModel(IContainer container, SitesDBContext db)
        {
            this.container = container;
            this.db = db;
            Exports = container.GetAll<IExport>();
            SelectedExport = Exports.First();
        }

        /// <summary>
        /// 当前小说
        /// </summary>
        public BookInfo CurrentBook { get; set; }

        /// <summary>
        /// 导出功能集合
        /// </summary>
        public IEnumerable<IExport> Exports { get; private set; }

        /// <summary>
        /// 选中导出目录
        /// </summary>
        public string SelectFolder { get; set; }

        /// <summary>
        /// 选中导出功能
        /// </summary>
        public IExport SelectedExport { get; set; }

        /// <summary>
        /// 导出文件名称
        /// </summary>
        public string ExportFileName { get; set; }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseDialog()
        {
            CurrentBook = null;
            SelectedExport = Exports.First();
            ExportFileName = string.Empty;
            SelectFolder = string.Empty;
            ((RadWindow)View).Close();
        }

        /// <summary>
        /// 开始导出
        /// </summary>
        public void DoExport()
        {
            using (var bookDB = new BookDBContext(CurrentBook.Name))
            {
                if (bookDB.Chapters.Any(a => string.IsNullOrEmpty(a.Content)))
                {
                    RadWindow.Confirm(new DialogParameters
                    {
                        Header = "提示",
                        Content = "当前小说存在空白章节，需要先下载空白章节吗？",
                        OkButtonContent = "继续",
                        CancelButtonContent = "取消",
                        Closed = async (s, e) =>
                        {
                            if (e.DialogResult ?? false)
                            {
                                await container.Get<ShellViewModel>().StartBusy(() =>
                                {
                                    HtmlWeb web = new HtmlWeb();
                                    var site = db.Sites.Single(a => a.ID == CurrentBook.CurrentSiteID);
                                    var doc = web.Load(CurrentBook.CurrentSource);
                                    var chapterNodes = doc.DocumentNode.SelectNodes(@"//" + site.ChapterNode);
                                    foreach (var chapterNode in chapterNodes)
                                    {
                                        var chapterName = chapterNode.SelectSingleNode(site.ChapterNameNode)?.InnerText;
                                        var chapter = bookDB.Chapters.SingleOrDefault(a => a.BookID == CurrentBook.ID && a.Title == chapterName);
                                        var url = chapterNode.SelectSingleNode(site.ChapterUrlNode).GetAttributeValue("href", string.Empty);
                                        url = WebHelper.Combine(new Uri(CurrentBook.CurrentSource), url);
                                        if (chapter != null && string.IsNullOrEmpty(chapter.Content))
                                        {
                                            var contentDoc = web.Load(url);
                                            var content = contentDoc.DocumentNode.SelectSingleNode(@"//" + site.ContentNode).InnerText;
                                            content = "　　" + string.Join("\r\n　　", content.Split('\r', '\n').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().Replace("　　", "\r\n　　")));
                                            chapter.Content = content;
                                            bookDB.SaveChanges();
                                        }
                                        else if (chapter == null)
                                        {
                                            var contentDoc = web.Load(url);
                                            var content = contentDoc.DocumentNode.SelectSingleNode(@"//" + site.ContentNode).InnerText;
                                            content = "　　" + string.Join("\r\n　　", content.Split('\r', '\n').Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim().Replace("　　", "\r\n　　")));
                                            bookDB.Chapters.Add(new TB_Chapter
                                            {
                                                BookID = CurrentBook.ID,
                                                Title = chapterName,
                                                CurrentSRC = url,
                                                SiteID = site.ID,
                                                Content = content
                                            });
                                            bookDB.SaveChanges();
                                        }
                                    }
                                }, "正在下载所有空白章节");
                            }
                        }
                    });
                }
                ExecuteExport();
            }
        }

        /// <summary>
        /// 执行导出
        /// </summary>
        private void ExecuteExport()
        {
            using (var bookDB = new BookDBContext(CurrentBook.Name))
            {
                var chapterList = bookDB.Chapters.ToList().Select(a => Mapper.Map<ChapterInfo>(a)).ToList();
                if (SelectedExport.Export(ExportFileName, CurrentBook, chapterList))
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Header = "提示",
                        Content = "生成电子书成功！",
                        OkButtonContent = "确定",
                        Owner = container.Get<ShellView>()
                    });
                }
                else
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Header = "提示",
                        Content = "生成电子书失败！",
                        OkButtonContent = "确定",
                        Owner = container.Get<ShellView>()
                    });
                }
                CloseDialog();
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(CurrentBook):
                case nameof(SelectFolder):
                case nameof(SelectedExport):
                    if (CurrentBook != null)
                    {
                        if (!string.IsNullOrEmpty(SelectFolder))
                        {
                            ExportFileName = Path.Combine(SelectFolder, CurrentBook.Name + SelectedExport.Extension);
                        }
                        else
                        {
                            ExportFileName = CurrentBook.Name + SelectedExport.Extension;
                        }
                    }

                    break;
            }
        }
    }
}
