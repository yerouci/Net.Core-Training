using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Entities.Models;
using Entities.ModelsDTO;

namespace Entities.Mappings
{
    public class BookProfile: Profile
    {
        public BookProfile() 
        {
            CreateMap<Book, BookInputDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, BookDetailsDTO>()
                .ForMember(f => f.Title, opt => opt.MapFrom(m => m.Title))
                .ForMember(f => f.ISBN, opt => opt.MapFrom(m => m.ISBN))
                .ForMember(f => f.Date, opt => opt.MapFrom(m => m.Date));                
        }
    }
}
