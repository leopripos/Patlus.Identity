using AutoMapper;
using Patlus.Identity.UseCase.Entities;
using static Patlus.Identity.Rest.Features.Accounts.AccountDto;

namespace Patlus.Identity.Rest.Features.Accounts
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
