namespace BuildMentor.Services.Base
{
	public interface IDbService<TEntity>
	{
		public void Add(TEntity entity);

		public void Update(TEntity entity);

		public void Delete(TEntity entity);

		public void Delete(int id);

		public TEntity Get(int id);

		public List<TEntity> GetAll();
		
	}
}
