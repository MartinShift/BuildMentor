using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
	public class ToolMaintenanceService : BaseDbService<ToolMaintenanceRecord>, IDbService<ToolMaintenanceRecord>
	{
		public ToolMaintenanceService(BuildContext context) : base(context)
		{
		}
		public void Add(ToolMaintenanceRecord entity)
		{
			dbSet.Add(entity);
			context.SaveChanges();
		}

		public void Delete(ToolMaintenanceRecord entity)
		{
			dbSet.Remove(entity);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			Delete(dbSet.Find(id));
		}

		public ToolMaintenanceRecord Get(int id)
		{
			return GetAll().First(x => x.Id == id);
		}

		public List<ToolMaintenanceRecord> GetAll()
		{
			return dbSet.Include(x => x.Tool).Include(x=> x.Document).ToList();
		}

		public void Update(ToolMaintenanceRecord entity)
		{
			dbSet.Update(entity);
			context.SaveChanges();
		}

	}
}
