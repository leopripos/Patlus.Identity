using AutoMapper;
using Patlus.IdentityManagement.UseCase.Features.Identities.CreateHosted;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateActiveStatus;
using Patlus.IdentityManagement.UseCase.Features.Identities.UpdateOwnPassword;

namespace Patlus.IdentityManagement.IntegrationDispatcher.Dispatchers.Identities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ActiveStatusUpdatedNotification, ActiveStatusUpdatedNotificationDto>()
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Entity.Id));

            CreateMap<HostedCreatedNotification, HostedCreatedNotificationDto>()
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Entity.Id));

            CreateMap<OwnPasswordUdpatedNotification, OwnPasswordUdpatedNotificationDto>()
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Entity.Id));
        }
    }
}
