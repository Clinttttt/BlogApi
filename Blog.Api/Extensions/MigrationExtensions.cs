using Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                Console.WriteLine("🔄 Checking for pending migrations...");
                var pendingMigrations = dbContext.Database.GetPendingMigrations().ToList();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine($"📋 Found {pendingMigrations.Count} pending migration(s):");
                    foreach (var migration in pendingMigrations)
                    {
                        Console.WriteLine($"   - {migration}");
                    }

                    Console.WriteLine("⚙️ Applying migrations...");
                    dbContext.Database.Migrate();
                    Console.WriteLine("✅ Migrations applied successfully!");
                }
                else
                {
                    Console.WriteLine("✅ Database is up to date - no migrations needed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Migration failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Re-throw to prevent app from starting with bad DB
            }
        }
    }
}