using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class COEDTO
    {
        public class input
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }        
        }
        public class temp
        {
            public int month_id { get; set; }
            public int year { get; set; }
            public Array event_list { get; set; }
        }
        
        public class eventl
        {
          //  public string ASMAY_Year { get; set; }
           // public int COEE_Id { get; set; }
            public string COEE_EStartDate { get; set; }
            public string COEE_EEndDate { get; set; }
            public string COEME_EventName { get; set; }
        }
        public Array monthlist { get; set; }
    }
}
