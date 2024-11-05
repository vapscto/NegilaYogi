using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_LocationDTO : CommonParamDTO
    {
        public long HRMLO_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMLO_LocationName { get; set; }
        public string HRMLO_LocationDesc { get; set; }
        public long HRMLO_CreatedBy { get; set; }
        public long HRMLO_UpdatedBy { get; set; }
        public bool HRMLO_ActiveFlg { get; set; }
        public string retrunMsg { get; set; }
        public long userid { get; set; }
        public Array locationList { get; set; }
        public Array institutionlist { get; set; }
    }

}