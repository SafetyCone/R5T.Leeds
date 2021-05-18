using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using R5T.Suebia;
using R5T.Suebia.Hastings;


namespace R5T.Leeds.Construction
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.TestSecretsDirectoryPathProvider();
        }

        private static void TestSecretsDirectoryPathProvider()
        {
            var serviceProvider = Program.GetServiceProvider();

            var secretsDirectoryPathProvider = serviceProvider.GetRequiredService<ISecretsDirectoryPathProvider>();

            var secretsDirectoryPath = secretsDirectoryPathProvider.GetSecretsDirectoryPath();

            Console.WriteLine($"Secrets directory path: {secretsDirectoryPath}");
        }

        private static IServiceProvider GetServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddMachineLocationAwareSecretsDirectoryPathProvider()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
