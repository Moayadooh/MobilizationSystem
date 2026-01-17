using MobilizationSystem.Domain;

namespace MobilizationSystem.Infrastructure
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(MobilizationDbContext context)
        {
            if (!context.Persons.Any())
            {
                context.Persons.AddRange(
                    new Person { FullName = "Muayad Salim", NationalId = "P001", IsAvailable = true },
                    new Person { FullName = "Eyad Salim", NationalId = "P002", IsAvailable = true }
                );
            }

            if (!context.Resources.Any())
            {
                context.Resources.AddRange(
                    new Resource { Name = "Truck 1", Type = "Vehicle", IsAvailable = true },
                    new Resource { Name = "Generator A", Type = "Equipment", IsAvailable = true }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
