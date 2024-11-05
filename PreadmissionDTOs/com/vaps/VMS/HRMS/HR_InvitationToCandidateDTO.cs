using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class HR_InvitationToCandidateDTO : CommonParamDTO
    {
        public long HRITC_Id { get; set; }
        public long HRC_Id { get; set; }
        public string HRITC_From { get; set; }
        public string HRITC_To { get; set; }
        public string HRITC_CC { get; set; }
        public string HRITC_BCC { get; set; }
        public string HRITC_Subject { get; set; }
        public string HRITC_Template { get; set; }
        public string HRITC_Attachments { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }

}