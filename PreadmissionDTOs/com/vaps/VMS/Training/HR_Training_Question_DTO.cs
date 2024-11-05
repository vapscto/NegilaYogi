using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Question_DTO
    {
        public long HRTRQNS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRMFQNS_Id { get; set; }
        public bool HRTRQNS_ActiveFlg { get; set; }
        public long? HRTRQNS_CreatedBy { get; set; }
        public long? HRTRQNS_UpdatedBy { get; set; }
        public DateTime? HRTRQNS_CreatedDate { get; set; }
        public DateTime? HRTRQNS_UpdatedDate { get; set; }


        public long userId { get; set; }
        public long HRTFEED_TrainerId { get; set; }
        public long hqn_id { get; set; }
        public long HRMFOPT_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTCR_PrgogramName { get; set; }
        public string HRTFEED_AboutTraining { get; set; }
        public string HRTFEED_Improvement { get; set; }
        public string HRTFEED_Response { get; set; }
        public string HRMFOPT_OptionName { get; set; }
        public string HRMFQNS_QuestionName { get; set; }
        public string employeename { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRTCR_InternalORExternalFlg1 { get; set; }
        public bool HRTCR_InternalORExternalFlg { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string returnvalue { get; set; }
        public bool HRTCR_ActiveFlag { get; set; }
        public bool returnval { get; set; }
        public Array training_question_mapping_list { get; set; }
        public Array question_list { get; set; }
        public Array training_list { get; set; }
        public Array training_question_details_list { get; set; }
        public Array question_details_list { get; set; }
        public Array question_view_list { get; set; }
        public Array question_emp_details_list { get; set; }
        public Array feedback_question { get; set; }
        public Array feedback_option { get; set; }
        public option[] question_Option { get; set; }
        public questionoption[] questionoption_new { get; set; }


        public class option
        {
            public long HRMFQNS_Id { get; set; }
        }
        public class questionoption
            {
            public long HRMFQNS_Id { get; set; }
            public long HRMFOPT_Id { get; set; }
        }

    }
}
