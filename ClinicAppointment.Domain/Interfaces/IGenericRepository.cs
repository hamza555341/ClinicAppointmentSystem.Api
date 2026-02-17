using ClinicAppointment.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Interfaces
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? criteria = null,
         params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);    


    }
}
