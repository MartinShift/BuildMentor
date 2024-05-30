using BuildMentor.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Database
{
    public class DataSeed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly BuildContext _context;

        public DataSeed(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, BuildContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public  List<User> GetDefaultUsers()
        {
            return new List<User>
            {
                new User
                {
                    Name = "Test Admin",
                    UserName = "Admin",
                    Email = "smtp2807@gmail.com",
                    Address = "UzNU",
                    Country = "Ukraine",
                    City = "Uzhhorod",
                    BirthDate = new DateOnly(2004, 07, 28),
                    Job = Enums.BuilderJobs.Architect,
                },
                new User
                {
                    Name = "Test User",
                    UserName = "User",
                    Email = "test1234@gmail.com",
                    Address = "Newton st. 1",
                    Country = "USA",
                    City = "New York",
                    BirthDate = new DateOnly(2002, 11, 11),
                    Job = Enums.BuilderJobs.Builder,
                },
                new User
                {
                    Name = "Test User 2",
                    UserName = "User2",
                    Email = "test1@gmail.com",
                    Address = "Oxford st 123",
                    Country = "UK",
                    City = "London",
                    BirthDate = new DateOnly(1999, 02, 22),
                    Job = Enums.BuilderJobs.Builder,
                }

            };
        }

        public List<IdentityRole<int>> GetDefaultRoles()
        {
            return new List<IdentityRole<int>>
            {
                new IdentityRole<int>
                {
                    Name = "ADMIN",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<int>
                {
                    Name = "USER",
                    NormalizedName = "USER"
                }
            };
        }

        public async Task SeedData()
        {
            _context.Database.EnsureCreated();
            _context.Database.Migrate();
            var roles = GetDefaultRoles(); 
            _context.UserRoles.RemoveRange(_context.UserRoles);
            _context.Users.RemoveRange(_context.Users);
            _context.Roles.RemoveRange(_context.Roles);
            await _context.SaveChangesAsync();
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }

            var users = GetDefaultUsers();
            await _userManager.CreateAsync(users[0], "Admin1234");
            await _userManager.AddToRoleAsync(users[0], "ADMIN");
            await _userManager.CreateAsync(users[1], "User1234");
            await _userManager.AddToRoleAsync(users[1], "USER");
            await _userManager.CreateAsync(users[2], "User21234");
            await _userManager.AddToRoleAsync(users[2], "USER");
            await _context.SaveChangesAsync();
        }


    }
}
