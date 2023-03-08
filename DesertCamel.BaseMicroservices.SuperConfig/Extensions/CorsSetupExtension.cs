namespace DesertCamel.BaseMicroservices.SuperConfig.Extensions
{
    public static class CorsSetupExtension
    {
        public static void UseSuperConfigCorsPolicy(this IApplicationBuilder app)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        }
    }
}
