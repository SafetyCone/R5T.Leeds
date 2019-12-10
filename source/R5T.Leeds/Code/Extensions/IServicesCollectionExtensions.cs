using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Scotia.Extensions;
using R5T.Suebia;
using R5T.Suebia.Alamania;
using R5T.Suebia.Default;
using R5T.Suebia.Hastings;


namespace R5T.Leeds
{
    public static class IServicesCollectionExtensions
    {
        public static IServiceCollection UseMachineLocationAwareCustomSecretsDirectoryPath(this IServiceCollection services)
        {
            services
                .AddAlamaniaSecretsDirectoyPathProviderServiceDependencies()
                .AddSingleton<AlamaniaSecretsDirectoryPathProvider>() // Add service directly since it is consumed directly, instead of through the ISecretsDirectoryPathProvider interface.
                .AddSingleton<ISecretsDirectoryPathProvider, MachineLocationAwareSecretsDirectoryPathProvider>()
                .AddSingleton<ISecretsFilePathProvider, DefaultSecretsFilePathProvider>()
                ;

            return services;
        }
    }
}
