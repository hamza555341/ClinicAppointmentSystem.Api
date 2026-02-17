using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Persistence.DbContexts;
using ClinicAppointment.Persistence.IdentityData.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ClinicAppointmentSystemApi.Extension
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDataBaseAsync(this WebApplication app)

        {
            await using var Scope = app.Services.CreateAsyncScope();

            var dbContext = Scope.ServiceProvider.GetRequiredService<ClinicAppointmentsDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> MigrateIdentityDataBaseAsync(this WebApplication app)

        {
            await using var Scope = app.Services.CreateAsyncScope();

            var dbContext = Scope.ServiceProvider.GetRequiredService<ClinicAppointmentsIdentityDbContext>();
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await dbContext.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerObj = Scope.ServiceProvider.GetRequiredKeyedService<IDataIntializer>("Default");
            await DataIntializerObj.InitializeDataAsync();

            return app;

        }

        public static async Task<WebApplication> SeedIdentityDataAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerObj = Scope.ServiceProvider.GetRequiredKeyedService<IDataIntializer>("Identity");
            await DataIntializerObj.InitializeDataAsync();

            return app;

        }



    }
}
