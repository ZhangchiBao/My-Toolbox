using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

namespace BookReading
{
    public class BookContext : DbContext
    {
        public BookContext() : base(new SQLiteConnection($"data source={Path.Combine(App.APP_FLODER, "shelf.db")}"), true)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new BookContextInitializer(modelBuilder));
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryAlias> CategoryAliases { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Chapter> Chapters { get; set; }
    }

    [Table("Chapters")]
    public class Chapter
    {
        [Key]
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 章节序号
        /// </summary>
        [Column("Index")]
        public int Index { get; set; }

        /// <summary>
        /// 小说ID
        /// </summary>
        [Column("BookID")]
        public string BookID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 段落
        /// </summary>
        [Column("Sections")]
        public string Sections { get; set; }

        /// <summary>
        /// 当前地址
        /// </summary>
        [Column("URL")]
        public string URL { get; set; }

        /// <summary>
        /// 是否已下载
        /// </summary>
        [Column("Downloaded")]
        public bool Downloaded { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        [Column("FinderKey")]
        public string FinderKey { get; set; }
    }

    [Table("Book")]
    public class Book
    {
        [Key]
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [Column("CategoryID")]
        public string CategoryID { get; set; }

        /// <summary>
        /// 小说名称
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Column("Author")]
        public string Author { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Column("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 封面地址
        /// </summary>
        [Column("CoverURL")]
        public string CoverURL { get; set; }

        /// <summary>
        /// 封面内容
        /// </summary>
        [Column("CoverContent")]
        public string CoverContent { get; set; }

        /// <summary>
        /// 当前更新地址
        /// </summary>
        [Column("URL")]
        public string URL { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        [Column("FinderKey")]
        public string FinderKey { get; set; }

        /// <summary>
        /// 章节
        /// </summary>
        public List<Chapter> Chapters { get; set; }
    }

    [Table("Category")]
    public class Category
    {
        [Key]
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Column("CategoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 别名集合
        /// </summary>
        public List<CategoryAlias> Alias { get; set; }

        /// <summary>
        /// 小说集合
        /// </summary>
        public List<Book> Books { get; set; }
    }

    [Table("CategoryAlias")]
    public class CategoryAlias
    {
        [Key]
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 所属分类ID
        /// </summary>
        [Column("CategoryID")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [Column("AliasName")]
        public string AliasName { get; set; }
    }
}
