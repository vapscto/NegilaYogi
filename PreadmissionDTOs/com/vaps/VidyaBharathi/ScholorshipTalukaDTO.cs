using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Scholorship
{
   public  class ScholorshipTalukaDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public string returnval { get; set; }
        public long IVRMMT_Id { get; set; }
        public string IVRMMT_Name { get; set; }
        public bool? IVRMMT_ActiveFlag { get; set; }
        public long IVRMMD_Id { get; set; }
        public bool? IVRMMT_AllowScholashipFlg { get; set; }
        public long? IVRMMT_MaxScholarshipQuota { get; set; }

    }
}
