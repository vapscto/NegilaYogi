using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Feedback_Qns_DTO
    {
        public long HRMFQNS_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMFQNS_QuestionName { get; set; }
        public string HRMFQNS_QuestionTypeFlg { get; set; }
        public int HRMFQNS_QuestionOrder { get; set; }
        public bool HRMFQNS_ActiveFlg { get; set; }
        public long? HRMFQNS_CreatedBy { get; set; }
        public long? HRMFQNS_UpdatedBy { get; set; }
        public DateTime? HRMFQNS_CreatedDate { get; set; }
        public DateTime? HRMFQNS_UpdatedDate { get; set; }
        public string HRMFQNS_QuestionForFlg { get; set; }

        public long userId { get; set; }
        public string returnvalue { get; set; }
        public bool returnval { get; set; }
        public Array question_list { get; set; }
        public Array question_details_list { get; set; }
    }
}
