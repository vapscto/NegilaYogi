using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_DocsUpload")]
    public class IVRM_DocsUploadDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IDU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long Login_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string IDU_Type { get; set; }
        public string IDU_Remarks { get; set; }
        public string IDU_Attachment { get; set; }
        public string IDU_FilePath { get; set; }
        public bool IDU_ActiveFlag { get; set; }
    }
}
