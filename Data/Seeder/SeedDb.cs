using HankoSpa.Services.Interfaces;

namespace HankoSpa.Data.Seeder
{
    public class SeedDb
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userServices;


        public SeedDb(AppDbContext context, IUserService usersService)
        {
            _context = context;
            _userServices = usersService;
        }

        public async Task SeedAsync()
        {
            await new PermissionsSeeder(_context).SeedAsync();
            await new UserRolesSeeder(_context, _userServices).SeedAsync();

        }
    }
}
