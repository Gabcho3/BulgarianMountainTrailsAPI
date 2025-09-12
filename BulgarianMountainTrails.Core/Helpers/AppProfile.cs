using AutoMapper;
using BulgarianMountainTrails.Core.DTOs;
using BulgarianMountainTrails.Data.Entities;

namespace BulgarianMountainTrails.Core.Helpers
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

            CreateMap<TrailDto, Trail>()
                .ForMember(dest => dest.Difficulty,
                opt => opt.MapFrom(src => Enum.Parse<Data.Enums.DifficultyEnum>(src.Difficulty, true)));

            CreateMap<HutDto, Hut>();

            CreateMap<TrailHutDto, TrailHut>();
        }
    }
}
