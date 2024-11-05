using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class StudentFeedbackFormDTO
    {
        public bool returnval { get; set; }
        public string message { get; set; }

        public long MI_Id { get; set; }
        public long ASGFE_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASGFE_FeedBack { get; set; }
        public DateTime? ASGFE_FeedbackDate { get; set; }
        public bool ASGFE_ActiveFlag { get; set; }
        public long ASGFE_CreatedBy { get; set; }
        public long ASGFE_UpdatedBy { get; set; }

        public Array instname { get; set; }
        public Array get_feedback { get; set; }

        

    }
}