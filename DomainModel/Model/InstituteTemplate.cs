using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Institution_Template")]
    public class InstituteTemplate : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMIT_Id { get; set; }
        public long IVRMT_Id { get; set; }
        public long IVRMIT_MI_Id { get; set; }
        public long IVRMIT_Category_Id { get; set; }
        public bool IVRMIT_ActiveFlag { get; set; }
        public bool IVRMIT_DeleteFlag { get; set; }
    }
}
