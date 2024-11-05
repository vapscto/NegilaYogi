using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_Master_PriorityDTO : CommonParamDTO
    {
        public long HRMPR_Id { get; set; }
        public string HRMP_Name { get; set; }
        public int HRMP_Order { get; set; }
        public bool HRMP_ActiveFlag { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public Array PriorityList { get; set; }
        public Array institutionlist { get; set; }
        public long MI_Id { get; set; }
        public long HRMP_CreatedBy { get; set; }
        public long HRMP_UpdatedBy { get; set; }
        public long userid { get; set; }
    }

}