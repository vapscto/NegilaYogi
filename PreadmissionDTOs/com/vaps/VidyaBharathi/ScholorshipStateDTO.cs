using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Scholorship
{
   public class ScholorshipStateDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public string returnval { get; set; }
        public long IVRMMS_Id { get; set; }
        public string IVRMMS_Name { get; set; }
        public int IVRMMS_Default { get; set; }
        public string IVRMMS_Code { get; set; }
        public long IVRMMC_Id { get; set; }
        public long? IVRMMS_CreatedBy { get; set; }
        public long? IVRMMS_UpdatedBy { get; set; }
        public bool? IVRMMS_ActiveFlag { get; set; }
        public bool? IVRMMS_AllowScholashipFlg { get; set; }
        public long? IVRMMS_MaxScholarshipQuota { get; set; }


    }
}
