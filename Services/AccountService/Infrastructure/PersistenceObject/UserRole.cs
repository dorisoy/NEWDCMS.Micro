using InfrastructureBase.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PersistenceObject
{
    public class UserRole: PersistenceObjectBase
    {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
    }
}
