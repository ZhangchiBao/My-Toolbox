using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca_del_Papa.DAL
{
    public class DBContext : DbContext
    {
        public DBContext() : base(new SqlConnection()
        {
            ConnectionString = new SqlConnectionStringBuilder()
            {
                ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bdp.mdf")}\";Integrated Security=True;Connect Timeout=30"
            }.ConnectionString
        }, true)
        {

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
        }

        public DbSet<Finder> Finders { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryAlias> CategoryAliases { get; set; }

        public DbSet<Book> Books { get; set; }
    }

    public class Book
    {
        [Key]
        public int ID { get; set; }

        public int CategoryID { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public int CurrentFinderID { get; set; }
        public string CurrentURL { get; set; }
    }

    public class CategoryAlias
    {
        [Key]
        public int ID { get; set; }

        public int CategoryID { get; set; }

        public string AliasName { get; set; }
    }

    public class Category
    {
        [Key]
        public int ID { get; set; }

        public string CategoryName { get; set; }

        public List<CategoryAlias> Alias { get; set; }

        public List<Book> Books { get; set; }
    }

    public class Finder
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public Guid Key { get; set; }
    }
}
