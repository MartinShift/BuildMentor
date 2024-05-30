using BuildMentor.Database;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services.Base
{
	public abstract class BaseDbService<TEntity> where TEntity : class
	{
		protected readonly DbSet<TEntity> dbSet;
		protected readonly BuildContext context;
		public BaseDbService(BuildContext context)
		{
			this.context = context;
			dbSet = context.Set<TEntity>();
		}
	}
}
