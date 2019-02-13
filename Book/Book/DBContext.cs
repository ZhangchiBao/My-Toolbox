using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "book.db" };
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);

            optionsBuilder.UseSqlite(connection);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<TB_Book> Books { get; set; }

        public DbSet<TB_Chapter> Chapters { get; set; }

        public DbSet<TB_Site> Sites { get; set; }
    }

    [Table("Books")]
    public class TB_Book
    {
        [Key]
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public string Author { get; set; }
        
        public string Description { get; set; }
        
        public string Type { get; set; }

        public string CurrentSource { get; set; }

        public int CurrentSiteID { get; set; }
    }

    [Table("Chapters")]
    public class TB_Chapter
    {
        [Key]
        public int ID { get; set; }
        
        public int BookID { get; set; }
        
        public string Title { get; set; }

        public string CurrentSRC { get; set; }

        public string Content { get; set; }
    }

    [Table("Sites")]
    public class TB_Site
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string URL { get; set; }

        public string BookNameNode { get; set; }

        public string AuthorNode { get; set; }

        public string BookURLNode { get; set; }

        public string SearchURL { get; set; }
        public uint? SearchSize { get; set; }

        public string BookResultsNode { get; set; }

        public string DescriptionNode { get; set; }

        public string UpdateNode { get; set; }

        /// <summary>
        /// 章节节点
        /// </summary>
        public string ChapterNode { get; set; }
        
        public string ChapterNameNode { get; set; }
        
        public string ChapterUrlNode { get; set; }

        public string ContentNode { get; set; }
    }
}
