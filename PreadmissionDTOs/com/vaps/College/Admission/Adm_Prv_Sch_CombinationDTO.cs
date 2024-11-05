using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class Adm_Prv_Sch_CombinationDTO
    {
        public long ADMCB_ID { get; set; }
        public long MI_Id { get; set; }
        public string ADMCB_NAME { get; set; }
        public bool ADMCB_Activeflag { get; set; }
        public Array getdetails { get; set; }
        public Array editdetails { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }

    }
}
