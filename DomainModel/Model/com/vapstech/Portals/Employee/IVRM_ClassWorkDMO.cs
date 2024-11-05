using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_Assignment")]
   public class IVRM_ClassWorkDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICW_Id { get; set; }
        public long MI_Id { get; set; }
        public string ICW_Topic { get; set; }
        public string ICW_SubTopic { get; set; }
        public string ICW_Content { get; set; }
        public long ICW_TeachingAid { get; set; }
        public bool ICW_ActiveFlag { get; set; }
        public DateTime ICW_FromDate { get; set; }
        public DateTime ICW_ToDate { get; set; }
        public string ICW_Assignment { get; set; }
        public string ICW_Evaluation { get; set; }
        public string ICW_Attachment { get; set; }
        public string ICW_FilePath { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long Login_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public List<IVRM_ClassWork_Attatchment_DMO> IVRM_ClassWork_Attatchment_DMO { get; set; }
    }
}
