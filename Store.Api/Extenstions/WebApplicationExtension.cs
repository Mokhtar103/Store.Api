using Domain.Contract;

namespace Store.Api.Extenstions
{
    public static class WebApplicationExtension
    {

        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInitializer.InitiliazeAsync();
            await dbInitializer.InitiliazeIdentityAsync();

            return app;
        }
    }
}
