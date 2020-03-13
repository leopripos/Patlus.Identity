using AutoMapper;
using Patlus.IdentityManagement.UseCase.Entities;

namespace Patlus.IdentityManagement.Rest.Features.Me
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Identity, ProfileDto>();
        }
    }
}
