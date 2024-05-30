using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Models;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildMentor.Services
{
	public class ToolService :  BaseDbService<Tool>, IDbService<Tool>
	{
		public ToolService(BuildContext context) : base(context)
		{
		}

		public void Add(Tool entity)
		{
			dbSet.Add(entity);
			context.SaveChanges();
		}

		public void Update(Tool entity)
		{
			dbSet.Update(entity);
			context.SaveChanges();
		}

		public void Delete(Tool entity)
		{
			dbSet.Remove(entity);
			context.SaveChanges();
		}
		
		public void Delete(int id)
		{
			Delete(dbSet.First(s=> s.Id == id));
		}

		public Tool Get(int id)
		{
			return GetAll().Find(s => s.Id == id);
		}

		public List<Tool> GetAll()
		{
			return dbSet
				.Include(x=> x.Instructions)
				.Include(x=> x.ToolPermissionRequests)
				.ThenInclude(x=> x.User)
				.Include(x=> x.ToolPermissionRequests)
				.Include(x=> x.ToolPermissions)
				.Include(x=> x.UserTools)
				.Include(x=> x.Image).ToList();
		}
		public List<Tool> GetFilterPage(FilterModel filter, int page)
		{
            return GetAll().Where(t =>
                (string.IsNullOrEmpty(filter.SearchText) ||
                t.Name.Contains(filter.SearchText) ||
                t.Description.Contains(filter.SearchText)) &&
                (filter.MinPrice == null || t.Price >= filter.MinPrice) &&
                (filter.MaxPrice == null || t.Price <= filter.MaxPrice)
            )
                .Skip((page - 1) * 40)
                .Take(40)
                .ToList();

        }

		public ToolUpdateModel MapToUpdateModel(Tool tool)
		{
			return new ToolUpdateModel()
			{
				Id = tool.Id,
				Description = tool.Description,
				Name = tool.Name,
				Price = tool.Price,
				UploadedImage = null,
				Image = tool.Image,
			};
		}
		public Tool MapToEntity(ToolModel model)
		{
			return new Tool()
			{
				Description = model.Description,
				Name = model.Name,
				Price = model.Price
			};
		}
		public Tool MapUpdateToEntity(ToolUpdateModel model)
		{
			return new Tool()
			{
				Id = model.Id,
				Description = model.Description,
				Name = model.Name,
				Price = model.Price,
				Image = model.Image,
			};
		}
    }
}
