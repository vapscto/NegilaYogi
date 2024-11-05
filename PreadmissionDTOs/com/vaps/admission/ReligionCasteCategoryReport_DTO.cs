using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class ReligionCasteCategoryReport_DTO
    {
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array castecategorylist { get; set; }
        public Array religionlist { get; set; }
        public Array reportlist { get; set; }
        public long ASMCL_Id { get; set; }
        public long IMCC_Id { get; set; }
        public long IVRMMR_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string flag { get; set; }
        public string ZERO { get; set; }
        public Array reportlist2 { get; set; }
        public ReligionCasteCategoryReport_DTO[] selectedclasslist { get; set; }
        public ReligionCasteCategoryReport_DTO[] selectedcastecategorylist { get; set; }
        public ReligionCasteCategoryReport_DTO[] selectedreligionlist { get; set; }
    }
}
