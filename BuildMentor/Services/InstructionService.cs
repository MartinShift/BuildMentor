using BuildMentor.Database;
using BuildMentor.Database.Entities;
using BuildMentor.Models;
using BuildMentor.Services.Base;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BuildMentor.Services
{
	public class InstructionService : BaseDbService<Instruction>, IDbService<Instruction>, IValidate<InstructionModel>
	{
		private readonly DbSet<ExternalResource> externalResources;
		public InstructionService(BuildContext context) : base(context)
		{
			this.externalResources = context.Set<ExternalResource>();
		}
		public void Add(Instruction entity)
		{
			dbSet.Add(entity);
			context.SaveChanges();
		}

		public void Delete(Instruction entity)
		{
			dbSet.Remove(entity);
			context.SaveChanges();
		}

		public void Delete(int id)
		{
			Delete(dbSet.Find(id));
		}

		public Instruction Get(int id)
		{
			return GetAll().First(x=> x.Id == id);
		}

		public List<Instruction> GetAll()
		{
			return dbSet.Include(x => x.Tool)
				.Include(x=> x.ExternalResources)
				.ToList();
		}

		public List<Instruction> GetByProduct(int productId)
		{
			return dbSet
				.Include(x=> x.Tool)
				.Include(x=> x.ExternalResources)
				.Where(x=> x.ToolId == productId)
				.ToList();
		}
		public void Update(Instruction entity)
		{
			dbSet.Update(entity);
            externalResources.AddRange(entity.ExternalResources);
            context.SaveChanges();
		}

		public void Update(InstructionModel model)
		{
            var entity = Get(model.Id ?? 0);
            entity.LastUpdatedDate = DateTime.Now;
			entity.Title = model.Title;
			entity.Description = model.Description;
			entity.ExternalResources.Clear();
            entity.ExternalResources = model.ExternalResources.ToList();		
            Update(entity);
        }

		public Instruction MapToEntity(InstructionModel model)
		{
			return new Instruction()
			{
				CreatedDate = model.CreatedDate ?? DateTime.Now,
				LastUpdatedDate = model.LastUpdatedDate ?? DateTime.Now,
				Title = model.Title,
				Description = model.Description,
				ExternalResources = model.ExternalResources,
				Id = model.Id ?? 0,
				ToolId = model.ToolId,
			};
		}

        public InstructionModel MapToModel(Instruction model)
        {
            return new InstructionModel()
            {
                CreatedDate = model.CreatedDate,
                LastUpdatedDate = model.LastUpdatedDate,
                Title = model.Title,
                Description = model.Description,
                ExternalResources = model.ExternalResources.ToList(),
                Id = model.Id,
                ToolId = model.ToolId,
            };
        }
		public string Validate(InstructionModel model)
		{
            if (string.IsNullOrEmpty(model.Title))
            {
                return "Title cannot be Empty!";
            }
            if (string.IsNullOrEmpty(model.Description))
            {
                return "Description cannot be Empty!";
            }
            for (int i = 0; i < model.ExternalResources.Count; i++)
            {
                if (string.IsNullOrEmpty(model.ExternalResources[i].Title))
                {
                    return $"External link №{i + 1} has empty Title!";
                }
                if (!Regex.IsMatch(model.ExternalResources[i].Link, @"/((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/"))
                {
                    return $"External Link №{i + 1} has invalid Address!";
                }
            }
			return string.Empty;
        }
    }
}
