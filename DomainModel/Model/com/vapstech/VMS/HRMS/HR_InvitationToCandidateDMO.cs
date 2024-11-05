using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_InvitationToCandidate")]
    public class HR_InvitationToCandidateDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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