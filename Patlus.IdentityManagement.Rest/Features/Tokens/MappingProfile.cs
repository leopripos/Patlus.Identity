using AutoMapper;
using Patlus.IdentityManagement.UseCase.Entities;

namespace Patlus.IdentityManagement.Rest.Features.Tokens
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Token, TokenDto>()
                .ReverseMap();
        }
    }
}
