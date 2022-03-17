using FunctionAppNET6.Handler;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using FunctionAppNET6.Repo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FunctionAppNET6.Core.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Options;
using FunctionAppNET6.Core.Interfaces;

[assembly: FunctionsStartup(typeof(FunctionAppNET6.Startup))]
namespace FunctionAppNET6
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ExecutionContextOptions executioncontextoptions = builder.Services.BuildServiceProvider()
           .GetService<IOptions<ExecutionContextOptions>>().Value;
            string currentDirectory = executioncontextoptions.AppDirectory;

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(currentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            IConfigurationRoot config = configBuilder.Build();

            builder.Services.AddMediatR(typeof(GetJobDetailsHandler).Assembly);

            IConfigurationSection connectionStringSettings = config.GetSection("ConnectionStrings");
            builder.Services.Configure<ConnectionStrings>(connectionStringSettings);

            ConnectionStrings connectionStrings = new ConnectionStrings();
            connectionStringSettings.Bind(connectionStrings);

            builder.Services.AddTransient<IRepository, Repository>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   ConfigureDbContextOptionsBuilder(options, connectionStrings.RequestService),
               ServiceLifetime.Transient
           );
        }

        private void ConfigureDbContextOptionsBuilder(DbContextOptionsBuilder options, string connectionString)
        {
            options
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }
    }
}
