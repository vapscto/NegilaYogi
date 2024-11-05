using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Employee_AssesmentparameterDTO : CommonParamDTO
    {
        public long HR_Emp_As_paraid { get; set; }
        public long MI_Id { get; set; }
        public string HR_Emp_As_parameter { get; set; }
        public string HR_Emp_As_parameterdesc { get; set; }

        public bool HR_Emp_Assparameter_ActiveFlag { get; set; }
        public Array bankdetailList { get; set; }
        public string retrunMsg { get; set; }

        public long roleId { get; set; }

    }
}
