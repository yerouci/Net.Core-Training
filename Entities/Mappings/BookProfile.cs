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
        }
    }
}
