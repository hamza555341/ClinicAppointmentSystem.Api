using ClinicAppointment.Domain.Entites;
using ClinicAppointment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Service.Specifications
{
    internal abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity,TKey> 
        where TEntity : BaseEntity<TKey>    
    {


        #region Criteria

        protected BaseSpecification(Expression<Func<TEntity,bool>> criteriaExp)
        {        
            Criteria = criteriaExp;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; }
        #endregion


        #region IncludeExepressions

        public ICollection<Expression<Func<TEntity, object>>> IncludeExepressions { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExepressions.Add(includeExp);
        }




        #endregion









    }
}
