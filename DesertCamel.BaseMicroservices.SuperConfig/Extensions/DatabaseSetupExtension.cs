using DesertCamel.BaseMicroservices.SuperConfig.EntityFramework;
using DesertCamel.BaseMicroservices.SuperConfig.Models.Configs;
using DesertCamel.BaseMicroservices.SuperConfig.Services;
using DesertCamel.BaseMicroservices.SuperConfig.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DesertCamel.BaseMicroservices.SuperConfig.Extensions
{
    public static class DatabaseSetupExtension
    {
        public static void AddSuperConfigProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var selectedProvider = configuration.GetSection(AppConstants.ConfigKeys.SELECTED_PROVIDER).Value;
            switch (selectedProvider)
            {
                case AppConstants.ProviderTypes.POSTGRES:
                    var pgConnectionString = configuration.GetSection(AppConstants.ConfigKeys.POSTGRES_DB_CONN_STRING).Value;
                    services.AddDbContext<SuperConfigDbContext, PgSuperConfigDbContext>(options =>
                    {
                        options.UseNpgsql(pgConnectionString);
                    });
                    services.AddScoped<IConfigService, SqlConfigService>();
                    break;
                case AppConstants.ProviderTypes.AWS_DYNAMODB:
                    services.Configure<AwsDynamoDbConfig>(configuration.GetSection(AwsDynamoDbConfig.AwsDynamoDbSection));
                    services.AddScoped<IConfigService, AwsDynamoDbConfigService>();
                    break;
                case AppConstants.ProviderTypes.GOOGLE_FIRESTORE:
                    throw new Exception("No implementation yet");
                default:
                    throw new Exception("Unknown Selected Database");
            }
        }

        public static void RunSuperConfigProviderMigration(this IApplicationBuilder app, IConfiguration configuration)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var selectedProvider = configuration.GetSection(AppConstants.ConfigKeys.SELECTED_PROVIDER).Value;
                switch (selectedProvider)
                {
                    case AppConstants.ProviderTypes.POSTGRES:
                        var pgDb = scope.ServiceProvider.GetRequiredService<PgSuperConfigDbContext>().Database;
                        if (pgDb.GetPendingMigrations().Any())
                        {
                            pgDb.Migrate();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
