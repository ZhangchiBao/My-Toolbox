using AutoMapper;
using BookReading.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReading
{
    public static class DTOMapper
    {
        static DTOMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Chapter, ChapterShowModel>()
                .ForMember(c => c.ID, c => c.MapFrom(option => new Guid(option.ID)));

                cfg.CreateMap<Book, BookShowModel>()
                .ForMember(b => b.ID, b => b.MapFrom(option => new Guid(option.ID)))
                .ForMember(b => b.Name, b => b.MapFrom(option => option.BookName))
                .ForMember(b => b.Cover, b => b.MapFrom(option => option.CoverURL))
                .ForMember(b => b.Chapters, b => b.MapFrom(option => option.Chapters.Select(c => mapper.Map<ChapterShowModel>(c)).ToList()));

                cfg.CreateMap<Category, CategoryShowModel>()
                .ForMember(c => c.ID, c => c.MapFrom(option => new Guid(option.ID)))
                .ForMember(c => c.Books, c => c.MapFrom(option => option.Books.Select(b => mapper.Map<BookShowModel>(b)).ToList()))
                .ForMember(c => c.Name, c => c.MapFrom(option => option.CategoryName));
            });
            mapper = config.CreateMapper();
        }

        private static IMapper mapper;

        public static TDestination Map<TDestination>(object source)
        {
            return mapper.Map<TDestination>(source);
        }
    }
}
