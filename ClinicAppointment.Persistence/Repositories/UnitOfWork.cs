using ClinicAppointment.Domain.Entites;
using ClinicAppointment.Domain.Interfaces;
using ClinicAppointment.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicAppointmentsDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];
        public UnitOfWork(ClinicAppointmentsDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
          var EntityType= typeof(TEntity);

            if (_repositories.TryGetValue(EntityType, out object? Repo))
                return (IGenericRepository<TEntity,TKey>)Repo;

            var NewRepo = new GenericRepository<TEntity, TKey>(_dbContext);

            _repositories[EntityType] = NewRepo;
            return NewRepo; 

        }

        public async Task<int> SaveChangesAsync()
        {
           return await _dbContext.SaveChangesAsync(); 
        }
    }
}
