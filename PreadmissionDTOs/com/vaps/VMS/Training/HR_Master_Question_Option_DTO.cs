using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Question_Option_DTO
    {
        public long HRMQNOP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMFQNS_Id { get; set; }
        public long HRMFOPT_Id { get; set; }
        public bool HRMQNOP_ActiveFlg { get; set; }
        public long? HRMQNOP_CreatedBy { get; set; }
        public long? HRMQNOP_UpdatedBy { get; set; }
        public DateTime? HRMQNOP_CreatedDate { get; set; }
        public DateTime? HRMQNOP_UpdatedDate { get; set; }


        public long userId { get; set; }
        public string HRMFQNS_QuestionName { get; set; }
        public string returnvalue { get; set; }
        public bool HRMFQNS_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public long hqn_id { get; set; }
        public string HRMFOPT_OptionName { get; set; }
        public bool HRMFOPT_ActiveFlg { get; set; }
        public Array question_option_list { get; set; }
        public Array option_list { get; set; }
        public Array question_list { get; set; }
        public Array question_details_list { get; set; }
        public Array option_details_list { get; set; }
        public Array option_view_list { get; set; }
       
        public HR_Master_Feedback_Option_new[] Feedback_Option { get; set; }
        

        public class HR_Master_Feedback_Option_new
        {
            public long HRMFOPT_Id { get; set; }
        }


    }
}
