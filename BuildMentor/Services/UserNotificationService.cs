using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
    public class UserNotificationService : BaseDbService<UserNotification>, IDbService<UserNotification>
    {
        public UserNotificationService(BuildContext context) : base(context)
        {
        }
        public void Add(UserNotification entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Delete(UserNotification entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(dbSet.Find(id));
        }

        public UserNotification Get(int id)
        {
            return GetAll().First(x => x.Id == id);
        }

        public List<UserNotification> GetAll()
        {
            return dbSet.Include(x => x.User)
                .ThenInclude(x=> x.Avatar)
                .Include(x => x.Admin)
                .ThenInclude(x=> x.Avatar).ToList();
        }

        public void Update(UserNotification entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
