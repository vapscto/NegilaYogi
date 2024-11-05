using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_QuarterDTO : CommonParamDTO
    {
        public long HRMQ_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMQ_QuarterName { get; set; }
        public string HRMQ_QuarterDescription { get; set; }
        public DateTime? HRMQ_FromDay { get; set; }
        public string HRMQ_FromMonth { get; set; }
        public DateTime? HRMQ_ToDay { get; set; }
        public string HRMQ_ToMonth { get; set; }
        public bool HRMQ_ActiveFlag { get; set; }
        public Array bankdetailList { get; set; }
        public string retrunMsg { get; set; }
        public Array leaveyeardropdown { get; set; }

        public Array monthdropdown { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public long HRMQ_CreatedBy { get; set; }
        public long HRMQ_UpdatedBy { get; set; }
    }
}
