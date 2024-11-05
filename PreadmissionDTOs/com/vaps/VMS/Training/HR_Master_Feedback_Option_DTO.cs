using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Feedback_Option_DTO
    {
        public long HRMFOPT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMFOPT_OptionName { get; set; }
        public int HRMFOPT_OptionOrder { get; set; }
        public bool HRMFOPT_ActiveFlg { get; set; }
        public long? HRMFOPT_CreatedBy { get; set; }
        public long? HRMFOPT_UpdatedBy { get; set; }
        public DateTime? HRMFOPT_CreatedDate { get; set; }
        public DateTime? HRMFOPT_UpdatedDate { get; set; }
        public string HRMFOPT_OptionFor { get; set; }
        public long userId { get; set; }
        public bool returnval { get; set; }
        public string returnvalue { get; set; }
        public Array feedback_option_list { get; set; }
        public Array feedback_option_details_list { get; set; }
    }
}
