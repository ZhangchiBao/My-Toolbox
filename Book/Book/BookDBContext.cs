using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Book
{
    public class BookDBContext : DbContext
    {
        private readonly string bookName;

        public BookDBContext(string bookName)
        {
            this.bookName = bookName;
            if (!Directory.Exists(App.SHELF_DIRECTORY))
            {
                Directory.CreateDirectory(App.SHELF_DIRECTORY);
            }
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = $"{App.SHELF_DIRECTORY}\\{bookName}.db" };
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            optionsBuilder.UseSqlite(connection);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<TB_Chapter> Chapters { get; set; }
    }

    [Table("Chapters")]
    public class TB_Chapter
    {
        [Key]
        public int ID { get; set; }

        public int BookID { get; set; }

        public string Title { get; set; }

        public string CurrentSRC { get; set; }

        public int SiteID { get; set; }

        public string Content { get; set; }
    }
}
