using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BookReading
{
    public class BookContextInitializer : SqliteCreateDatabaseIfNotExists<BookContext>
    {
        public BookContextInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Seed(BookContext context)
        {
            #region 初始化分类数据
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "玄幻奇幻",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "玄幻"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "奇幻"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "东方奇幻"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "玄幻奇幻"
                    }
                }
            });
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "武侠修真",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "武侠"
                    },
                    new CategoryAlias {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "仙侠"
                    },
                    new CategoryAlias {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "武侠修真"
                    }
                }
            });
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "现代都市",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "都市"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "现实"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "现代都市"
                    }
                }
            });
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "历史军事",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "历史"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "军事"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "历史军事"
                    }
                }
            });
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "游戏竞技",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "游戏"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "体育"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "游戏竞技"
                    }
                }
            });
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "科幻灵异",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "科幻"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "悬疑灵异"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "灵异"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "悬疑"
                    },
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "科幻灵异"
                    }
                }
            });
            context.Categories.Add(new Category
            {
                ID = Guid.NewGuid().ToString(),
                CategoryName = "其他",
                Alias = new List<CategoryAlias>
                {
                    new CategoryAlias
                    {
                        ID = Guid.NewGuid().ToString(),
                        AliasName = "其他"
                    }
                }
            });
            context.SaveChanges(); 
            #endregion


        }
    }
}
