using Microsoft.EntityFrameworkCore;
using SmartRecruitmentMatchingPlatform.API.Data.Context;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Data.Seed
{
    public static class EmployerSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Check whether employer data already exists
            var employerExists = await context
                .Set<Employer>()
                .AnyAsync();

            if (employerExists)
            {
                return;
            }

            /*
             * Sample Employer data is not added here yet.
             * UserId must match an existing User record.
             *
             * Employer properties:
             * EmployerId
             * UserId
             * CompanyName
             * CompanyDescription
             * Location
             * Website
             */

            await context.SaveChangesAsync();
        }
    }
}