using ClinicAppointment.Domain.Entites;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> :
        IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly ClinicAppointmentsDbContext _dbContext;

        public GenericRepository(ClinicAppointmentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)=>
            await _dbContext.Set<TEntity>().AddAsync(entity);


        public void Delete(TEntity entity)=>
                 _dbContext.Set<TEntity>().Remove(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? criteria = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (criteria != null)
                query = query.Where(criteria);

            if (includes != null && includes.Any())
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)=>
            await _dbContext.Set<TEntity>().FindAsync(id);


        public void Update(TEntity entity)=>
                 _dbContext.Set<TEntity>().Update(entity);

    }
}
