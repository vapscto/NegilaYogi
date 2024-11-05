using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_EmployeeTypeDTO:CommonParamDTO
    {
        public long HRMET_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMET_EmployeeType { get; set; }
        public bool HRMET_ActiveFlag { get; set; }
        public Array employeeTypeList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
    }
}
