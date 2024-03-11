using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Infrastructure.Mappings
{
    using CodingTest.Core.Models;

    public class RainfallMappingProfile : Profile
    {
        public RainfallMappingProfile()
        {
            CreateMap<RainfallApiResponse, RainfallDto>()
                .ForMember(dest => dest.MeasurementDateTime, opt => opt.MapFrom(src => src.Items.Measures.LatestReading.DateTime))
                .ForMember(dest => dest.RainfallAmount, opt => opt.MapFrom(src => src.Items.Measures.LatestReading.Value));
        }
    }
}
