using CompilerTest.Compiling;
using CompilerTest.Compiling.InstructionSet;
using CompilerTest.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CompilerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = false;
                });
            });

            services.AddSingleton<IConfigurationManager, Configuration.ConfigurationManager>();
            services.AddSingleton<IComponentProvider, ComponentProvider>();
            services.AddTransient<App>();
        }
    }
}
