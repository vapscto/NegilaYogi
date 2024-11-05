using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class IVRM_Master_GenderDTO:CommonParamDTO
    {
        public long IVRMMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public bool IVRMMG_ActiveFlag { get; set; }
        public Array genderList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
    }
}
