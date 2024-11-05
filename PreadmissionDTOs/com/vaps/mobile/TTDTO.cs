using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class TTDTO
    {
        public class input
        {
            public long ASMAY_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMCL_Id { get; set; }
            public long ASMS_Id { get; set; }
        }
        public class temp
        {
            public string CategoryName { get; set; }
            public string ClassName { get; set; }
            public string SectionName { get; set; }        
        }
        public class events_list
        {
            public string DayName { get; set; }
            public Array TTData { get; set; }
        }
        public class eventl
        {  
            public string PeriodName { get; set; }
            public string staffName { get; set; }          
            public string SubjectName { get; set; }
        }
       

        public Array TT_common { get; set; }    
        public Array TimeTable { get; set; }

    }
}
