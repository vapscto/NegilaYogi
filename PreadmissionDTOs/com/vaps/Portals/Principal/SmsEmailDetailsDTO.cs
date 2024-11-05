using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class SmsEmailDetailsDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }

        public DateTime frmdate { get; set; }
        public DateTime todate { get; set; }
        public string type { get; set; }
        public string templete { get; set; }
        public Array griddata { get; set; }
        public Array monthlist { get; set; }
        public Array smsmodulelist { get; set; }
    }
}

