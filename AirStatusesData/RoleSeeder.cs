using AirStatusesDomain;

namespace AirStatusesData
{
    public class RoleSeeder
    {
        private readonly AppDbContext _context;

        public RoleSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role { Id = 1, Code = "Reader" });
                _context.Roles.Add(new Role { Id = 2, Code = "Writer" });

                await _context.SaveChangesAsync();
            }
        }
    }
}
