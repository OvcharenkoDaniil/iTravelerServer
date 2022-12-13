using System.Linq;
using AutoMapper;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.FlightVM;


namespace iTravelerServer.Domain.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // CreateMap<FlightListVM,Flight>()
            //     .ForMember(f=>f.);

            // CreateMap<Property, PropertyListDto>()
            //     .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
            //     .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
            //     .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            //     .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name))
            //     .ForMember(d => d.Photo, opt => opt.MapFrom(src => src.Photos
            //                     .FirstOrDefault(p => p.IsPrimary).ImageUrl)); 


            
        }
        
    }
}