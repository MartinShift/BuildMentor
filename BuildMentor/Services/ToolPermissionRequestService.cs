using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
	public class ToolPermissionRequestService : BaseDbService<ToolPermissionRequest>, IDbService<ToolPermissionRequest>
	{
		public ToolPermissionRequestService(BuildContext context) : base(context)
		{

		}

		public void Add(ToolPermissionRequest entity)
		{
			dbSet.Add(entity);
			context.SaveChanges();
		}

		public void Delete(ToolPermissionRequest entity)
		{
			dbSet.Remove(entity);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			Delete(dbSet.Find(id));
		}

		public ToolPermissionRequest Get(int id)
		{
			return GetAll().First(x => x.Id == id);
		}

		public List<ToolPermissionRequest> GetAll()
		{
			return dbSet
				.Include(x => x.Tool)
				.ThenInclude(x=> x.Image)
				.Include(x => x.User)
				.ToList();
		}

		public void Update(ToolPermissionRequest entity)
		{
			dbSet.Update(entity);
			context.SaveChanges();
		}
	}
}
