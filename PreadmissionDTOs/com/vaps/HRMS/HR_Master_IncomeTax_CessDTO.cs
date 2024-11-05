using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_IncomeTax_CessDTO:CommonParamDTO
    {
        public long HRMITC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMITC_CessName { get; set; }
        public bool HRMITC_ActiveFlag { get; set; }
        public Array incometax_cessList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
    }
}
