using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
	public class ToolPermissionService : BaseDbService<ToolPermission>, IDbService<ToolPermission>
	{
		public ToolPermissionService(BuildContext context) : base(context)
		{
		}

		public void Add(ToolPermission entity)
		{
			dbSet.Add(entity);
			context.SaveChanges();
		}

		public void Delete(ToolPermission entity)
		{
			dbSet.Remove(entity);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			Delete(dbSet.Find(id));
		}

		public ToolPermission Get(int id)
		{
			return GetAll().First(x => x.Id == id);
		}

		public List<ToolPermission> GetAll()
		{
			return dbSet
				.Include(x => x.Tool)
				.ThenInclude(x=> x.Image)
				.Include(x => x.User).Include(x => x.Admin).ToList();
		}

		public void Update(ToolPermission entity)
		{
			dbSet.Update(entity);
			context.SaveChanges();
		}
	}
}
