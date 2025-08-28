using AutoMapper;
using BulgarianMountainTrailsAPI.Data.Models;
using BulgarianMountainTrailsAPI.DTOs;

namespace BulgarianMountainTrailsAPI.Helpers
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Trail, TrailDto>()
                .ForMember(dest => dest.Difficulty,
                opt => opt.MapFrom(src => src.Difficulty.ToString()))
                .ForMember(dest => dest.Huts, opt => opt.MapFrom(opt => opt.TrailHuts.Select(th => th.Hut)));

            CreateMap<Hut, HutDto>()
                .ForMember(dest => dest.Trails, opt => opt.MapFrom(opt => opt.TrailHuts.Select(th => th.Trail)));

            CreateMap<Hut, SimpleHutDto>();
            CreateMap<Trail, SimpleTrailDto>();
        }
    }
}
