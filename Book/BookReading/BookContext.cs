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
            //#region 初始化数据库
            //if (Database.CreateIfNotExists())
            //{
            //    Categories.Add(new Category { CategoryName = "玄幻奇幻", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "玄幻" }, new CategoryAlias { AliasName = "奇幻" }, new CategoryAlias { AliasName = "玄幻奇幻" } } });
            //    Categories.Add(new Category { CategoryName = "武侠修真", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "武侠" }, new CategoryAlias { AliasName = "仙侠" }, new CategoryAlias { AliasName = "武侠修真" } } });
            //    Categories.Add(new Category { CategoryName = "现代都市", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "都市" }, new CategoryAlias { AliasName = "现实" }, new CategoryAlias { AliasName = "现代都市" } } });
            //    Categories.Add(new Category { CategoryName = "历史军事", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "历史" }, new CategoryAlias { AliasName = "军事" }, new CategoryAlias { AliasName = "历史军事" } } });
            //    Categories.Add(new Category { CategoryName = "游戏竞技", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "游戏" }, new CategoryAlias { AliasName = "体育" }, new CategoryAlias { AliasName = "游戏竞技" } } });
            //    Categories.Add(new Category { CategoryName = "科幻灵异", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "科幻" }, new CategoryAlias { AliasName = "悬疑灵异" }, new CategoryAlias { AliasName = "灵异" }, new CategoryAlias { AliasName = "悬疑" }, new CategoryAlias { AliasName = "科幻灵异" } } });
            //    Categories.Add(new Category { CategoryName = "其他", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "其他" } } });
            //    SaveChanges();
            //}
            //#endregion
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<BookContext>(modelBuilder));
            Database.SetInitializer(new BookContextInitializer(modelBuilder));
            //var model = modelBuilder.Build(Database.Connection);
            //IDatabaseCreator sqliteDatabaseCreator = new SqliteDatabaseCreator();
            //sqliteDatabaseCreator.Create(Database, model);
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
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public Guid ID { get; set; }

        /// <summary>
        /// 小说ID
        /// </summary>
        [Column("BookID")]
        public int BookID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 章节地址
        /// </summary>
        [Column("FilePath")]
        public string FilePath { get; set; }

        /// <summary>
        /// 是否已下载
        /// </summary>
        [Column("Downloaded")]
        public bool Downloaded { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        [Column("FinderKey")]
        public Guid FinderKey { get; set; }

        /// <summary>
        /// 当前地址
        /// </summary>
        [Column("URL")]
        public string URL { get; set; }
    }

    [Table("Book")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        [Column("CategoryID")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 小说名称
        /// </summary>
        [Column("BookName")]
        public string BookName { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Column("Author")]
        public string Author { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Column("Descption")]
        public string Descption { get; set; }

        /// <summary>
        /// 封面地址
        /// </summary>
        [Column("CoverURL")]
        public string CoverURL { get; set; }

        /// <summary>
        /// 当前更新地址
        /// </summary>
        [Column("URL")]
        public string URL { get; set; }

        /// <summary>
        /// 小说目录
        /// </summary>
        [Column("BookFloder")]
        public string BookFloder { get; set; }

        /// <summary>
        /// 搜索器Key
        /// </summary>
        [Column("FinderKey")]
        public Guid FinderKey { get; set; }

        /// <summary>
        /// 章节
        /// </summary>
        public List<Chapter> Chapters { get; set; }
    }

    [Table("Category")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

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
