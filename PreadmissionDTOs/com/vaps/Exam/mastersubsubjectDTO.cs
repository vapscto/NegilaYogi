using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam 
{
    public class mastersubsubjectDTO
    {
        public bool already_cnt { get; set; }
        public int EMSS_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EMSS_SubSubjectCode { get; set; }
        public string EMSS_Order { get; set; }
        public bool EMSS_ActiveFlag { get; set; }
        public Array editlist { get; set; }
        public Array getlist { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public mastersubsubjectDTO[] subsubjectDTO { get; set; }
    }
      
}
