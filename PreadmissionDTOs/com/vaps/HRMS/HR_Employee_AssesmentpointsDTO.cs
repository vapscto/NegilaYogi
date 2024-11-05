using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Employee_AssesmentpointsDTO : CommonParamDTO
    {
        public long HR_Emp_As_Opid { get; set; }
        public long MI_Id { get; set; }
        public string HR_Emp_As_Option { get; set; }
        public long HR_Emp_As_Points { get; set; }
        public bool HR_Emp_Asspoint_ActiveFlag { get; set; }
        public Array bankdetailList { get; set; }
        public string retrunMsg { get; set; }

        public long roleId { get; set; }

    }
}
