using AutoMapper;
using Entities.Models;
using Entities.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Mappings
{
    public class ReviewProfile: Profile
    {
        public ReviewProfile() 
        {
            CreateMap<Review, ReviewInputDTO>()
                .ReverseMap()
                .ForMember(f => f.Qualification, opt => opt.MapFrom(m => m.Qualification))
                .ForMember(f => f.Opinion, opt => opt.MapFrom(m => m.Opinion));
        }
    }
}
