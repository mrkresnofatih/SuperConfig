﻿using DesertCamel.BaseMicroservices.SuperBootstrap.Base;
using DesertCamel.BaseMicroservices.SuperConfig.Extensions;

namespace DesertCamel.BaseMicroservices.SuperConfig
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddSuperConfigProvider(Configuration);
            services.AddBootstrapBase(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.RunSuperConfigProviderMigration(Configuration);

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseBootstrapBase();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
