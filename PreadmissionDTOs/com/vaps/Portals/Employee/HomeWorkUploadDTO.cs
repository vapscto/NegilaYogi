using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
   public  class HomeWorkUploadDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public Array classlist { get; set; }

        public Array classreportlist { get; set; }

        public classarray1[] classarray { get; set; }

        public sectionarray1[] sectionarray { get; set; }


        public string month { get; set; }
        public string year { get; set; }
        public Array reportlist { get; set; }
      

        public Array acayear { get; set; }

        public Array Month_array { get; set; }

        public Array view_array { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string type { get; set; }
        public string flag { get; set; }

        public Array sectionlist { get; set; }

        public Array seen_unseenlist { get; set; }

        public long seen_Topicid { get; set; }

        public long seen_unseen { get; set; }


        public long ICW_Id { get; set; }
        public long IHW_Id { get; set; }
        public long Temp { get; set; }


        public string IHW_Attachment { get; set; }
        public string IHWATT_Attachment { get; set; }
        public string IHWATT_FileName { get; set; }

        public Array attachementlist { get; set; }
        public long Login_Id { get; set; }
        



        public class classarray1
        {
            public long ASMCL_Id { get; set; }
        }

        public class sectionarray1
        {
            public long ASMS_Id { get; set; }
        }
    }
}
