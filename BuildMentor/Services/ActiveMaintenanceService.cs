using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
    public class ActiveMaintenanceService : BaseDbService<ActiveMaintenance>, IDbService<ActiveMaintenance>
    {
        public ActiveMaintenanceService(BuildContext context) : base(context)
        {
        }

        public void Add(ActiveMaintenance entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Delete(ActiveMaintenance entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(dbSet.Find(id));
        }

        public ActiveMaintenance Get(int id)
        {
            return GetAll().First(x => x.Id == id);
        }

        public List<ActiveMaintenance> GetAll()
        {
            return dbSet.Include(x => x.UserTool).ToList();
        }

        public void Update(ActiveMaintenance entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
