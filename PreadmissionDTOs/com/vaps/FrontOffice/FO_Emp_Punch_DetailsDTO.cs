using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Emp_Punch_DetailsDTO
    {
        public long FOEPD_Id { get; set; }
        public long MI_Id { get; set; }


        public long FOEP_Id { get; set; }
        public string FOEPD_PunchTime { get; set; }
        public string FOEPD_InOutFlg { get; set; }
        public string FOEPD_Flag { get; set; }
    }
}
