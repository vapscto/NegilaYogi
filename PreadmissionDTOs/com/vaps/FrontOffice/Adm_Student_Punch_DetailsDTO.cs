using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class Adm_Student_Punch_DetailsDTO
    {
        public long ASPUD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASPU_Id { get; set; }
        public string ASPUD_PunchTime { get; set; }
        public string ASPUD_InOutFlg { get; set; }
    }
}
