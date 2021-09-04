using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }

        private static void SeedData(AppDbContext dbContext, bool isProduction)
        {
            if (isProduction)
            {
                try
                {
                    Console.WriteLine("Attempting to apply migations...");
                    dbContext.Database.Migrate();    
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("Apply of migration failed: " + ex.Message);
                }
            }

            if (!dbContext.Platforms.Any())
            {
                dbContext.Platforms.AddRange(
                    new Models.Platform()
                    {
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Models.Platform()
                    {
                        Name = "Sql Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Models.Platform()
                    {
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free"
                    }
                );

                dbContext.SaveChanges();

                Console.WriteLine("Database initialized!");
            }
            else
            {
                Console.WriteLine("Database already initialized!");
            }
        }
    }
}