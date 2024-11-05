using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class AppUserRoleDTO : CommonParamDTO
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public long RoleTypeId { get; set; }
    }
}
