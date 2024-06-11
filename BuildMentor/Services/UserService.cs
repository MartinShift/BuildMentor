using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Models;
using BuildMentor.Services.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BuildMentor.Services
{
	public class UserService : BaseDbService<User>, IDbService<User>
	{
		public UserService(BuildContext context) : base(context)
		{
		}
		public void Add(User entity)
		{
			dbSet.Add(entity);
			context.SaveChanges();
		}

		public void Delete(User entity)
		{
			dbSet.Remove(entity);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			Delete(dbSet.First(s=> s.Id == id));
		}

		public User Get(int id)
		{
			return GetAll().Find(s => s.Id == id);
		}

		public List<User> GetAll()
		{
			return GetIncluded()
				.ToList();
		}
		public IQueryable<User> GetIncluded()
		{
			return dbSet.Include(s => s.Avatar)
				.Include(x => x.ToolPermissionRequests)
				.Include(u => u.GivenPermissions)
				.Include(x => x.ReceivedNotifications)
				.Include(x => x.ToolPermissionRequests)
				.Include(x => x.UserTools)
				.ThenInclude(x => x.Tool)
				.ThenInclude(x => x.Image)
				.Include(x => x.UserTools)
				.ThenInclude(x => x.ToolMaintenanceRecords)
				.Include(x => x.UserTools)
				.ThenInclude(x => x.Notifications);

        }
		public void Update(User entity)
		{
			dbSet.Update(entity);
			context.SaveChanges();
		}

        public string GetFirstPartOfEmail(string email)
        {
            if (email != null)
            {
                string[] emailParts = email.Split('@');
                if (emailParts.Length >= 1)
                {
                    string firstPartOfEmail = emailParts[0];
                    return firstPartOfEmail;
                }
            }
            return "";
        }

		public UserProfile MapToProfile(User user, bool isAdmin)
		{
			return new UserProfile()
			{
				Id = user.Id,
				Login = user.UserName,
				Name = user.Name,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				AvatarUrl = user.Avatar == null ? "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1200px-No-Image-Placeholder.svg.png" : user.Avatar.Url(),
				City = user.City,
				Country = user.Country,
				IsAdmin = isAdmin,
				BirthDate = user.BirthDate.ToDateTime(TimeOnly.MinValue),
				Address = user.Address,
				Job = user.Job,
				ReceivedNotifications = user.ReceivedNotifications,
				ToolPermissions = user.ToolPermissions,
				GivenPermissions = user.GivenPermissions,
				ToolPermissionRequests = user.ToolPermissionRequests,

					
			};
		}
		public void Update(UserProfile profile)
		{
			   var user = Get(profile.Id);
			user.Name = profile.Name;
			user.Email = profile.Email;
			user.PhoneNumber = profile.PhoneNumber;
			user.City = profile.City;
			user.Country = profile.Country;
			user.Address = profile.Address;
			Update(user);
		}

		public async Task<IEnumerable<User>> GetUsersAsync(UserManager<User> userManager)
		{
            return (await GetIncluded().ToListAsync()).Where(x => userManager.IsInRoleAsync(x, "USER").Result);

        }
        public async Task<IEnumerable<User>> GetAdminsAsync(UserManager<User> userManager)
		{
            return (await GetIncluded().ToListAsync()).Where(x => userManager.IsInRoleAsync(x, "ADMIN").Result);

        }
    }
}
