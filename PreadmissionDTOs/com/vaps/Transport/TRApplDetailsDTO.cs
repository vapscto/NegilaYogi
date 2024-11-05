using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class TRApplDetailsDTO 
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string TYPE { get; set; }
        public string MSG { get; set; }
        public Array YearList { get; set; }
        public Array griddata { get; set; }
        public bool smscheck { get; set; }
        public bool emailcheck { get; set; }
        public Temp_sms_List[] Temp_sms_List { get; set; }

    }

    public class Temp_sms_List
    {
        public long AMST_Id { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string StudentName { get; set; }
   
       


    }
}
