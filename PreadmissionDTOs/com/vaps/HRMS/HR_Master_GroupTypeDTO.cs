using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_GroupTypeDTO :CommonParamDTO
    {
        public long HRMGT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }

        public string HRMGT_Code { get; set; }
        public Int32? HRMGT_Order { get; set; }
        public bool HRMGT_ActiveFlag { get; set; }
        public Array grouptypeList { get; set; }
        public string retrunMsg { get; set; }

        public long roleId { get; set; }

        public HR_Master_GroupTypeDTO[] GroupTypeDTO { get; set; }

    }
}
