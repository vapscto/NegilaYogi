using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class mastersubexamDTO
    {
        public bool already_cnt { get; set; }
        public long EMSE_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMSE_SubExamName { get; set; }
        public string EMSE_SubExamCode { get; set; }
        public string EMSE_SubExamOrder { get; set; }
        public string EMSE_ActiveFlag { get; set; }
        public Array editlist { get; set; }
        public Array getlist { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public string retrunMsg { get; set; }
        public mastersubexamDTO[] subexamDTO { get; set; }

    }
}
