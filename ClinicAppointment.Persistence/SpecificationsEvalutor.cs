using ClinicAppointment.Domain.Entites;
using ClinicAppointment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Persistence
{
    internal abstract  class SpecificationsEvalutor
    {

        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint,
            ISpecification<TEntity,TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint; // _dbContext.Set<TEntity>();

            if (specification is not null)
            { 

                 if (specification.Criteria is not null)
                 {
                   Query = Query.Where(specification.Criteria);
                 }   

                 if (specification.IncludeExepressions != null && specification.IncludeExepressions.Any())
                 { 
                
                      Query = specification.IncludeExepressions.Aggregate(Query,
                      (currentQuery, includeExp) => currentQuery.Include(includeExp));

                 }


            }



            return Query;


        }




    }
}
