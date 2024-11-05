using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_student_LocMapping", Schema = "TRN")]
    public class TR_student_LocMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRSLM_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long TRML_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool TRSLM_ActiveFlag { get; set; }

    }
}
