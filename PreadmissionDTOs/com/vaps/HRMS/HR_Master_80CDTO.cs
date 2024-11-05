using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_80CDTO :CommonParamDTO
    {
        public long HRMMM_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMMC_Name { get; set; }
        public string HRMMMC_Description { get; set; }
        public bool HRMMMC_ActiveFlag { get; set; }
        // public bool HRMBD_ActiveFlag { get; set; }
        public Array bankdetailList { get; set; }
        public string retrunMsg { get; set; }

        public long roleId { get; set; }

    }
}
