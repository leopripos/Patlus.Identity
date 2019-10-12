using AutoMapper;
using Patlus.IdentityManagement.UseCase.Entities;
using static Patlus.IdentityManagement.Rest.Features.Accounts.AccountDto;

namespace Patlus.IdentityManagement.Rest.Features.Accounts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(dto => dto.Hosted, opt => opt.ExplicitExpansion())
                .ReverseMap();

            CreateMap<HostedAccount, HostedAccount>()
                .ReverseMap();
        }
    }
}
