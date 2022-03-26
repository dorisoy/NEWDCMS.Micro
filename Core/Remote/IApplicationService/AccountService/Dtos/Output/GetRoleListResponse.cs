using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.AccountService.Dtos.Output
{
    public class GetRoleListResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool SuperAdmin { get; set; }
        public List<int> Permissions { get; set; }
    }
}
