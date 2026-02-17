using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicAppointment.Domain.Entites
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
    }
}
