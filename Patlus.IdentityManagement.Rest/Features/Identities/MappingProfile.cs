using AutoMapper;
using Patlus.IdentityManagement.UseCase.Entities;

namespace Patlus.IdentityManagement.Rest.Features.Identities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Identity, IdentityDto>()
                .ReverseMap();

            CreateMap<HostedAccount, HostedAccountDto>()
                .ForMember(e => e.CreatedTime, e => e.Ignore())
                .ReverseMap();
        }
    }
}
