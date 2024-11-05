using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
  public   class Adm_Student_College_PunchDTO
    {
        public long ASPU_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ASPU_PunchDate { get; set; }
        public bool ASPU_ActiveFlg { get; set; }
        public DateTime CreatedDate { get; set; }

        public long ASPUD_Id { get; set; }
        public string ASPUD_InOutFlg { get; set; }
        public string ASPUD_PunchTime { get; set; }
    }
}
