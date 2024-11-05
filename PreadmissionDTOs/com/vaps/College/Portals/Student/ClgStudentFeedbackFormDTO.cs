using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Portals
{
    public class ClgStudentFeedbackFormDTO
    {
        public bool returnval { get; set; }
        public string message { get; set; }

        public long ACSGFE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string ACSGFE_Feedback { get; set; }
        public DateTime? ACSGFE_FeedbackDate { get; set; }
        public bool ACSGFE_ActiveFlag { get; set; }
        public long ACSGFE_CreatedBy { get; set; }
        public long ACSGFE_UpdatedBy { get; set; }

        public Array instname { get; set; }
        public Array get_feedback { get; set; }

        

    }
}