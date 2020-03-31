using AutoMapper;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Create;
using Patlus.IdentityManagement.UseCase.Features.Tokens.Refresh;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Tokens
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatedNotification, CreatedNotificationDto>()
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Entity.Id))
                .ForMember(e => e.IdentityId, opt => opt.MapFrom(e => e.IdentityId))
                .ForMember(e => e.Scheme, opt => opt.MapFrom(e => e.Entity.Scheme))
                .ForMember(e => e.Access, opt => opt.MapFrom(e => e.Entity.Access))
                .ForMember(e => e.Refresh, opt => opt.MapFrom(e => e.Entity.Refresh));

            CreateMap<RefreshedNotification, RefreshedNotificationDto>()
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Entity.Id))
                .ForMember(e => e.IdentityId, opt => opt.MapFrom(e => e.IdentityId))
                .ForMember(e => e.Scheme, opt => opt.MapFrom(e => e.Entity.Scheme))
                .ForMember(e => e.Access, opt => opt.MapFrom(e => e.Entity.Access))
                .ForMember(e => e.Refresh, opt => opt.MapFrom(e => e.Entity.Refresh));
        }
    }
}
