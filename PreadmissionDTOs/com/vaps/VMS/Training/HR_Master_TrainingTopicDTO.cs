using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_TrainingTopicDTO : CommonParamDTO
    {
        public long HRMTT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMTT_Topic { get; set; }
        public bool HRMTT_ActiveFlg { get; set; }
        public long HRMTT_CreatedBy { get; set; }
        public long HRMTT_UpdatedBy { get; set; }
        public long userId { get; set; }
        public Array getmasterdata { get; set; }
        public Array edit_topic { get; set; }
        public string returnvalue { get; set; }
    }
}
