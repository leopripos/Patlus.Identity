using AutoMapper;
using Patlus.IdentityManagement.IntegrationDispatcher;

namespace Patlus.IdentityManagement.Presentation
{
    public static class MapperConfigurationExpressionExtension
    {
        public static IMapperConfigurationExpression AddIntegrationDispatcherMappings(this IMapperConfigurationExpression config)
        {
            config.AddMaps(IntegrationDispatcherModule.GetBundles());

            return config;
        }
    }
}
