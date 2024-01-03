using BussMaker.Entities;
using BussMaker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BussMaker.Infrastructure.Repository
{
    public class EFEntityRepository : IEntityRepository
    {
        private readonly BussMakerDbContext bussMakerDbContext;
        public EFEntityRepository(BussMakerDbContext bussMakerDbContext)
        {
            this.bussMakerDbContext = bussMakerDbContext;
        }

        public async Task CreateAsync(Entity entity)
        {
            await bussMakerDbContext.Entities.AddAsync(entity);
            await bussMakerDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingEntity = await bussMakerDbContext.Entities.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existingEntity != null)
            {
                bussMakerDbContext.Entities.Remove(existingEntity);
                await bussMakerDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Entity>> GetAllAsync()
        {
            return await bussMakerDbContext.Entities.AsNoTracking().ToListAsync();
        }

        public async Task<Entity> GetByIdAsync(int id)
        {
            return await bussMakerDbContext.Entities.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Entity entity)
        {
            bussMakerDbContext.Entities.Update(entity);
            await bussMakerDbContext.SaveChangesAsync();
        }
    }
}
