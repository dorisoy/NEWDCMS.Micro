using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base.AccessToken
{
    public class AccessTokenItem
    {
        public AccessTokenItem() { }
        public AccessTokenItem(int id, bool loginAdmin) { Id = id; LoginAdmin = loginAdmin; }
        public int Id { get; set; }
        public bool LoginAdmin { get; set; }
    }
}
