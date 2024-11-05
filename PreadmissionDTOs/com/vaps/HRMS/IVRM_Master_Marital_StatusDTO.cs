using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class IVRM_Master_Marital_StatusDTO:CommonParamDTO
    {
        public long IVRMMMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMMS_MaritalStatus { get; set; }
        public bool? IVRMMMS_ActiveFlag { get; set; }
        public Array maritalstatusList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
    }
}
