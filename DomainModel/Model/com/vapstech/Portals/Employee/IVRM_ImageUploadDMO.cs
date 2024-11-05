using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_ImageUpload")]
    public class IVRM_ImageUploadDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IIU_Id { get; set; }
        public long MI_Id { get; set; }
        public string IIU_Attachment { get; set; }
        public long IIU_Order { get; set; }
        public bool IIU_ActiveFlag { get; set; }

    }
}
