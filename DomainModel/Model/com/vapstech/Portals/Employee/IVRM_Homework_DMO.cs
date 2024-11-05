using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_HomeWork")]
    public class IVRM_Homework_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IHW_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IHW_AssignmentNo { get; set; }
        public long ISMS_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime? IHW_Date { get; set; }
        public string IHW_Topic { get; set; }
        public string IHW_Assignment { get; set; }
        public string IHW_Attachment { get; set; }
   
        public string IHW_FilePath { get; set; }
        public bool IHW_ActiveFlag { get; set; }

        public List<IVRM_HomeWork_Attatchment_DMO> IVRM_HomeWork_Attatchment_DMO { get; set; }

    }
}
