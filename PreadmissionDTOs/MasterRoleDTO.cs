using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterRoleDTO : CommonParamDTO
    {
        public long IVRMR_Id { get; set; }
        public string IVRMR_Role { get; set; }
        public string IVRMR_Role_desc { get; set; }
        public int IVRMR_Order { get; set; }

        public long maxcount { get; set; }
        public Array pagesdata { get; set; }
        public string returnval { get; set; }

        public string returnduplicatestatus { get; set; }


        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string roleType { get; set; }

        public int IVRMR_ActiveFlag { get; set; }

    }
}
