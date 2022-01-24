using AutoMapper;
using Entities.Models;
using Entities.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Mappings
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserInputDTO>().ReverseMap();
            CreateMap<User, UserDTO>()
                .ForMember(f => f.Id, opt => opt.MapFrom(m => m.Id.ToString()))
                .ForMember(f => f.Authors, opt => opt.MapFrom(m => m.Authors));
            CreateMap<User, UserListDTO>()
                .ForMember(f => f.AuthorsAmount, opt => opt.MapFrom(s => s.Authors.Count))
                .ReverseMap();
        }
    }
}
