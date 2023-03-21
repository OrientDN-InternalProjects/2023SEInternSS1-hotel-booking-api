using HotelBooking.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Test
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BookingDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<BookingDbContext>((_, context) => context.UseInMemoryDatabase("InMemoryDbForTesting"));

                // Build the service provider.
                var serviceProvider = services.BuildServiceProvider();

                //Create a scope to obtain a reference to the database
                using var scope = serviceProvider.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<Program>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();
                try
                {
                    // Seed the database with test data.
                    db.InitializeTestDatabase();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                }

            });
        }
    }
}