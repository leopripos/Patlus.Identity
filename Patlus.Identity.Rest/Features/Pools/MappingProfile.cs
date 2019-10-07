using AutoMapper;
using Patlus.Identity.UseCase.Entities;

namespace Patlus.Identity.Rest.Features.Pools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pool, PoolDto>()
                .ReverseMap();
        }
    }
}
