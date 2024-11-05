using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
   public class masterSpecialisationDTO
    {
        public long HRMSPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string HRMSPL_SpecialisationName { get; set; }
        public bool HRMSPL_ActiveFlg { get; set; }
        public long HRMSPL_CreatedBy { get; set; }
        public long HRMSPL_UpdatedBy { get; set; }
        public DateTime? HRMSPL_CreatedDate { get; set; }
        public DateTime? HRMSPL_UpdatedDate { get; set; }
        public Array alldata { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
    }
}
