using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Entities.Models;
using Entities.ModelsDTO;

namespace Entities.Mappings
{
    public class AuthorProfile: Profile
    {
        public AuthorProfile() 
        {
            CreateMap<Author, AuthorInputDTO>().ReverseMap();
            CreateMap<Author, AuthorDTO>();
            CreateMap<Author, AuthorDetailsDTO>()
                .ForMember(f => f.Name, opt => opt.MapFrom(m => m.Name))
                .ForMember(f => f.DateOfBirth, opt => opt.MapFrom(m => m.DateOfBirth))
                .ForMember(f => f.Nationality, opt => opt.MapFrom(m => m.Nationality))
                .ForMember(f => f.SubscribedUsersAmount, opt => opt.MapFrom(m => m.Users.Count));                
        }
    }
}
