using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Api.Extensions
{
    /// <summary>
    /// Extension used to seed the data
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Seeds the datadase
        /// </summary>
        /// <param name="app">Applicationn nbuilder</param>
        /// <returns>Applicationbuilder</returns>
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<LmsApiContext>();

                //db.Database.EnsureDeleted();
                //db.Database.Migrate();

                try
                {
                    await SeedData.InitAsync(db);
                }
                catch (Exception e)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(string.Join(" ", e.Message));
                    //throw;
                }
            }

            return app;
        }
    }
}
