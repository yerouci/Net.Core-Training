﻿using AutoMapper;
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
            CreateMap<User, UserDTO>();
            CreateMap<User, UserListDTO>()
                .ForMember(f => f.AuthorsAmount, opt => opt.MapFrom(s => s.Authors.Count))
                .ReverseMap();
        }
    }
}
