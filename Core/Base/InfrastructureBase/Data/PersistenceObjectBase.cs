using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.Data
{
    public abstract class PersistenceObjectBase
    {
        /// <summary>
        /// key
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}
