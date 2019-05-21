using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;

namespace Biblioteca_del_Papa.DAL
{
    public class DBContext : DbContext
    {
        public DBContext() : base(new SqlConnection()
        {
            ConnectionString = new SqlConnectionStringBuilder()
            {
                ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{Path.Combine(App.APPFloder, "bdp.mdf")}\";Integrated Security=True;Connect Timeout=30"
            }.ConnectionString
        }, true)
        {
            #region 初始化数据库
            if (Database.CreateIfNotExists())
            {
                Categories.Add(new Category { CategoryName = "玄幻奇幻", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "玄幻" }, new CategoryAlias { AliasName = "奇幻" }, new CategoryAlias { AliasName = "玄幻奇幻" } } });
                Categories.Add(new Category { CategoryName = "武侠修真", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "武侠" }, new CategoryAlias { AliasName = "仙侠" }, new CategoryAlias { AliasName = "武侠修真" } } });
                Categories.Add(new Category { CategoryName = "现代都市", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "都市" }, new CategoryAlias { AliasName = "现实" }, new CategoryAlias { AliasName = "现代都市" } } });
                Categories.Add(new Category { CategoryName = "历史军事", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "历史" }, new CategoryAlias { AliasName = "军事" }, new CategoryAlias { AliasName = "历史军事" } } });
                Categories.Add(new Category { CategoryName = "游戏竞技", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "游戏" }, new CategoryAlias { AliasName = "体育" }, new CategoryAlias { AliasName = "游戏竞技" } } });
                Categories.Add(new Category { CategoryName = "科幻灵异", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "科幻" }, new CategoryAlias { AliasName = "悬疑灵异" }, new CategoryAlias { AliasName = "灵异" }, new CategoryAlias { AliasName = "悬疑" }, new CategoryAlias { AliasName = "科幻灵异" } } });
                Categories.Add(new Category { CategoryName = "其他", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "其他" } } });
                SaveChanges();
            }
            #endregion
        }

        /// <summary>
        /// 分类
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// 分类别名
        /// </summary>
        public DbSet<CategoryAlias> CategoryAliases { get; set; }

        /// <summary>
        /// 小说
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// 小说章节
        /// </summary>
        public DbSet<Chapter> Chapters { get; set; }
    }

    /// <summary>
    /// 小说
    /// </summary>
    public class Book
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 小说名称
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 封面地址
        /// </summary>
        public string CoverURL { get; set; }

        /// <summary>
        /// 当前地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        public Guid FinderKey { get; set; }

        /// <summary>
        /// 章节
        /// </summary>
        public List<Chapter> Chapters { get; set; }
    }

    /// <summary>
    /// 章节
    /// </summary>
    public class Chapter
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 小说ID
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 章节内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        public Guid FinderKey { get; set; }

        /// <summary>
        /// 当前地址
        /// </summary>
        public string URL { get; set; }
    }

    /// <summary>
    /// 分类别名
    /// </summary>
    public class CategoryAlias
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 所属分类ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName { get; set; }
    }

    /// <summary>
    /// 分类
    /// </summary>
    public class Category
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
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
}
