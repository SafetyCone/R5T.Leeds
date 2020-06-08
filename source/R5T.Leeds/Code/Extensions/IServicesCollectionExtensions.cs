using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using R5T.Alamania;
using R5T.Bulgaria;
using R5T.Costobocia;
using R5T.Dacia;
using R5T.Hastings;
using R5T.Hastings.Default;
using R5T.Hastings.Default.Configuration;
using R5T.Lombardy;
using R5T.Macommania;
using R5T.Macommania.Default;
using R5T.Scotia.Extensions;
using R5T.Suebia;
using R5T.Suebia.Alamania;
using R5T.Suebia.Default;
using R5T.Suebia.Hastings;
using R5T.Visigothia;

using SuebiaHastingsIServiceCollectionExtensions = R5T.Suebia.Hastings.IServiceCollectionExtensions;


namespace R5T.Leeds
{
    public static class IServicesCollectionExtensions
    {
        public static 
            (
            IServiceAction<ISecretsDirectoryPathProvider> SecretsDirectoryPathProviderAction,
            (
            IServiceAction<IExecutableFileDirectoryPathProvider> ExecutableFileDirectoryPathProviderAction,
            IServiceAction<IExecutableFilePathProvider> ExecutableFilePathProviderAction,
            IServiceAction<IOptions<MachineConfiguration>> MachineConfigurationOptionsAction,
            IServiceAction<IMachineLocationProvider> MachineLocationProviderAction,
            (
            IServiceAction<IRivetOrganizationSecretsDirectoryPathProvider> RivetOrganizationSecretsDirectoryPathProviderAction,
            IServiceAction<ISecretsDirectoryFilePathProvider> SecretsDirectoryFilePathProviderAction,
            IServiceAction<ISecretsDirectoryPathProvider> SecretsDirectoryPathProviderAction,
            (
            IServiceAction<IDropboxDirectoryPathProvider> DropboxDirectoryPathProviderAction,
            IServiceAction<IOrganizationDirectoryNameProvider> OrganizationDirectoryNameProviderAction,
            IServiceAction<IOrganizationsStringlyTypedPathOperator> OrganizationsStringlyTypedPathOperatorAction,
            IServiceAction<IOrganizationStringlyTypedPathOperator> OrganizationStringlyTypedPathOperatorAction,
            IServiceAction<IRivetOrganizationDirectoryPathProvider> RivetOrganizationDirectoryPathProviderAction,
            IServiceAction<IUserProfileDirectoryPathProvider> UserProfileDirectoryPathProviderAction
            ) RivetOrganizationDirectoryPathProviderAction
            ) RivetOrganizationSecretsDirectoryPathProviderAction
            ) Dependencies
            )
        AddMachineLocationAwareSecretsDirectoryPathProviderAction(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            // -1.
#pragma warning disable IDE0042 // Deconstruct variable declaration
            var rivetOrganizationSecretsDirectoryPathProviderAction = services.AddRivetOrganizationSecretDirectoryFilePathProviderAction(
                stringlyTypedPathOperatorAction);
#pragma warning restore IDE0042 // Deconstruct variable declaration

            // 0.
            var executableFilePathProviderAction = services.AddDefaultExecutableFilePathProviderAction();
            var machineConfigurationOptionsAction = services.AddMachineConfigurationOptionsAction();

            // 1.
            var executableFileDirectoryPathProviderAction = services.AddDefaultExecutableFileDirectoryPathProviderAction(
                executableFilePathProviderAction,
                stringlyTypedPathOperatorAction);
            var machineLocationProviderAction = services.AddMachineLocationProviderAction(
                machineConfigurationOptionsAction);

            // 2.
            var secretsDirectoryPathProviderAction = SuebiaHastingsIServiceCollectionExtensions.AddMachineLocationAwareSecretsDirectoryPathProviderAction(services,
                executableFileDirectoryPathProviderAction,
                machineLocationProviderAction,
                rivetOrganizationSecretsDirectoryPathProviderAction.RivetOrganizationSecretsDirectoryPathProviderAction);

            services
                .Run(secretsDirectoryPathProviderAction)
                ;

            return
                (
                secretsDirectoryPathProviderAction,
                (
                executableFileDirectoryPathProviderAction,
                executableFilePathProviderAction,
                machineConfigurationOptionsAction,
                machineLocationProviderAction,
                rivetOrganizationSecretsDirectoryPathProviderAction
                )
                );
        }


        public static IServiceCollection AddMachineLocationAwareSecretsDirectoryPathProvider(this IServiceCollection services)
        {
            // 0.
            var machineConfigurationOptionsAction = services.AddMachineConfigurationOptionsAction();

            // 1.
            var machineLocationProviderAction = services.AddMachineLocationProviderAction(
                machineConfigurationOptionsAction);

            services
                .AddRivetOrganizationSecretsDirectoyPathProviderServiceDependencies()
                .AddSingleton<RivetOrganizationSecretsDirectoryPathProvider>() // Add service directly since it is consumed directly, instead of through the ISecretsDirectoryPathProvider interface.
                .AddSingleton<IExecutableFilePathProvider, ExecutableFilePathProvider>()
                .AddSingleton<IExecutableFileDirectoryPathProvider, ExecutableFileDirectoryPathProvider>()
                .AddSingleton<ISecretsDirectoryPathProvider, MachineLocationAwareSecretsDirectoryPathProvider>()
                .AddSingleton<ISecretsDirectoryFilePathProvider, SecretsDirectoryFilePathProvider>()

                .Run(machineLocationProviderAction)
                ;

            return services;
        }
    }
}
