using SQLite.CodeFirst;
using System.Collections.Generic;
using System.Data.Entity;

namespace BookReading
{
    public class BookContextInitializer : SqliteDropCreateDatabaseAlways<BookContext>
    {
        public BookContextInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Seed(BookContext context)
        {
            context.Categories.Add(new Category { CategoryName = "玄幻奇幻", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "玄幻" }, new CategoryAlias { AliasName = "奇幻" }, new CategoryAlias { AliasName = "玄幻奇幻" } } });
            context.Categories.Add(new Category { CategoryName = "武侠修真", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "武侠" }, new CategoryAlias { AliasName = "仙侠" }, new CategoryAlias { AliasName = "武侠修真" } } });
            context.Categories.Add(new Category { CategoryName = "现代都市", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "都市" }, new CategoryAlias { AliasName = "现实" }, new CategoryAlias { AliasName = "现代都市" } } });
            context.Categories.Add(new Category { CategoryName = "历史军事", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "历史" }, new CategoryAlias { AliasName = "军事" }, new CategoryAlias { AliasName = "历史军事" } } });
            context.Categories.Add(new Category { CategoryName = "游戏竞技", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "游戏" }, new CategoryAlias { AliasName = "体育" }, new CategoryAlias { AliasName = "游戏竞技" } } });
            context.Categories.Add(new Category { CategoryName = "科幻灵异", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "科幻" }, new CategoryAlias { AliasName = "悬疑灵异" }, new CategoryAlias { AliasName = "灵异" }, new CategoryAlias { AliasName = "悬疑" }, new CategoryAlias { AliasName = "科幻灵异" } } });
            context.Categories.Add(new Category { CategoryName = "其他", Alias = new List<CategoryAlias> { new CategoryAlias { AliasName = "其他" } } });
            context.SaveChanges();
        }
    }
}
