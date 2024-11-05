using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterRoleTypeDTO : CommonParamDTO
    {
        public long IVRMRT_Id { get; set; }
        public string IVRMRT_Role { get; set; }
        public long IVRMR_Id { get; set; }

        public string IVRMRT_RoleFlag { get; set; }
        public Array pagesdata { get; set; }

        public Array roledata { get; set; }
        public string returnval { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
        public Array studentDetails { get; set; }

        public string flag { get; set; }

    }
}
