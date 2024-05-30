using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
    public class UserToolService : BaseDbService<UserTool>, IDbService<UserTool>
    {
        public UserToolService(BuildContext context) : base(context)
        {
        }

        public void Add(UserTool entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Update(UserTool entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }

        public void Delete(UserTool entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(dbSet.First(s => s.Id == id));
        }

        public UserTool Get(int id)
        {
            return GetAll().First(s => s.Id == id);
        }

        public List<UserTool> GetAll()
        {
            return dbSet
                .Include(x=> x.Permission)
                .Include(x => x.ToolMaintenanceRecords)
                .ThenInclude(x=> x.Document)
                .Include(x => x.Tool)
                .ThenInclude(x => x.Instructions)
                .Include(x => x.Tool)
                .ThenInclude(x=> x.Image)
                .Include(x => x.User)
                .ThenInclude(x => x.Avatar)
                .Include(x => x.Notifications)
                .ThenInclude(x => x.Admin).ToList();
        }
    }
}
