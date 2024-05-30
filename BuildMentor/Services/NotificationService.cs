using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services;

public class NotificationService : BaseDbService<Notification>, IDbService<Notification>
{
	public NotificationService(BuildContext context) : base(context) 
	{

	}

	public void Add(Notification entity)
	{
		dbSet.Add(entity);
		context.SaveChanges();
	}

	public void Delete(Notification entity)
	{
		dbSet.Remove(entity);
		context.SaveChanges();
	}

	public void Delete(int id)
	{
		Delete(dbSet.Find(id));
	}

	public Notification Get(int id)
	{
		return GetAll().First(x=> x.Id == id);
	}

	public List<Notification> GetAll()
	{
		return dbSet
			.Include(x=> x.UserTool)
			.ThenInclude(x=> x.Tool)
			.Include(x=> x.UserTool)
			.ThenInclude(x=> x.ToolMaintenanceRecords)
			.Include(x=> x.UserTool)
			.ThenInclude(x=> x.User)

			.Include(x=> x.Admin).ToList();
	}

	public void Update(Notification entity)
	{
		dbSet.Update(entity);
		context.SaveChanges();
	}
}

