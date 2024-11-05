using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class IVRM_RemaindersDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ISES_Id { get; set; }
        public long ISESUSR_Id { get; set; }
        public string UserName { get; set; }
        public string ISES_Template_Name { get; set; }
        public string ISES_TemplateContent { get; set; }
        public string returnvalue { get; set; }
        public Array GetTemplateDetails { get; set; }
        public Array GetUserDetails { get; set; }
        public Array GetMappedUserDetails { get; set; }
        public bool? ISESUSR_ActiveFlg { get; set; }
        public Temp_UserMapping_Add[] Temp_UserMapping_Add { get; set; }
        public Temp_UserMapping_Remove[] Temp_UserMapping_Remove { get; set; }
    }

    public class Temp_UserMapping_Add
    {
        public long UserId { get; set; }         
        public long ISESUSR_Id { get; set; }
    }

    public class Temp_UserMapping_Remove
    {
        public long UserId { get; set; }
        public long ISESUSR_Id { get; set; }
    }
}
