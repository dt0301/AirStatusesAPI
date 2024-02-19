using AirStatusesDomain;
using Newtonsoft.Json;

namespace AirStatusesData
{
    public class UserSeeder
    {
        private readonly AppDbContext _context;

        public UserSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Users.Any())
            {
                var usersData = ReadUsersFromJson();
                _context.Users.AddRange(usersData);
                _context.SaveChanges();
            }
        }

        private List<User> ReadUsersFromJson()
        {
            // TODO: Get from configuration users.json
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
            var json = File.ReadAllText(path);
            var users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        }
    }
}
