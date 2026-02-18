using ClinicAppointment.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace ClinicAppointment.Domain.Interfaces
{
    public interface ISpecification<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExepressions { get; }
        public Expression<Func<TEntity,bool>> Criteria { get;}

      

    }
}
