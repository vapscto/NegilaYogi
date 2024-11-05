using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_ProgressCard_Remarks", Schema = "Exm")]
    public class Exm_ProgressCard_RemarksDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int ESPCR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EME_ID { get; set; }
        public long AMST_Id { get; set; }        
        public string EMER_Remarks { get; set; }  
        public bool EMER_ActiveFlag { get; set; }
    }
}
