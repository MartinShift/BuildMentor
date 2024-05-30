using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
    public class AdminRequestService : BaseDbService<AdminRequest>, IDbService<AdminRequest>
    {
        public AdminRequestService(BuildContext context) : base(context)
        {
        }

        public void Add(AdminRequest entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Delete(AdminRequest entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Delete(dbSet.Find(id));
        }

        public AdminRequest Get(int id)
        {
            return GetAll().First(x => x.Id == id);
        }

        public List<AdminRequest> GetAll()
        {
            return dbSet.Include(x => x.Sender).ToList();
        }

        public void Update(AdminRequest entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
    }
}
