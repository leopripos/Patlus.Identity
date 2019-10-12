using AutoMapper;
using Patlus.IdentityManagement.UseCase.Entities;

namespace Patlus.IdentityManagement.Rest.Features.Pools
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
