using ClinicAppointment.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();   
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>;

    }
}
