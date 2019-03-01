using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApp.Api
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseMySQL("server=144.34.221.50;database=library;user=root;password=1234qwer");
#else
            optionsBuilder.UseMySQL("server=127.0.0.1;database=library;user=root;password=1234qwer");
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TB_Site> Sites { get; set; }
    }

    [Table("Sites")]
    public class TB_Site
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string BookNameNode { get; set; }

        public string AuthorNode { get; set; }

        public string BookURLNode { get; set; }

        public string SearchURL { get; set; }

        public string BookResultsNode { get; set; }

        public string UpdateNode { get; set; }

        public string ChapterNode { get; set; }

        public string ChapterNameNode { get; set; }

        public string ChapterUrlNode { get; set; }

        public string ChapterTitleNode { get; set; }

        public string ChapterParagraphNode { get; set; }
    }
}
