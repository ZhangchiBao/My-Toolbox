using AutoMapper;
using BookReading.Entities;
using BookReading.Libs.Entity;
using Newtonsoft.Json;
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
                .ForMember(c => c.ID, c => c.MapFrom(option => new Guid(option.ID)))
                .ForMember(c => c.Sections, c => c.MapFrom(option => string.IsNullOrEmpty(option.Sections) ? null : JsonConvert.DeserializeObject<List<string>>(option.Sections)));

                cfg.CreateMap<Book, BookShowModel>()
                .ForMember(b => b.ID, c => c.MapFrom(option => new Guid(option.ID)))
                .ForMember(b => b.CoverContent, b => b.MapFrom(option => string.IsNullOrEmpty(option.CoverContent) ? null : Convert.FromBase64String(option.CoverContent)))
                .ForMember(b => b.CoverUrl, b => b.MapFrom(option => option.CoverURL))
                .ForMember(b => b.Chapters, b => b.MapFrom(option => option.Chapters.Select(c => mapper.Map<ChapterShowModel>(c)).OrderBy(a => a.Index).ToList()));

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
