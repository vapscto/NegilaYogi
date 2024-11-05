using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgMasterBranchDTO
    {
        public long AMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public string AMB_BranchInfo { get; set; }
        public string AMB_BranchType { get; set; }
        public int AMB_StudentCapacity { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public Array getdetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array editdetails { get; set; }
        public string AMB_AidedUnAided { get; set; }
        public List<orderlistbranch> orderlistbranch { get; set; }

    }

    public class orderlistbranch
    {
        public int AMB_Order { get; set; }
        public long AMB_Id { get; set; }
    }
}
