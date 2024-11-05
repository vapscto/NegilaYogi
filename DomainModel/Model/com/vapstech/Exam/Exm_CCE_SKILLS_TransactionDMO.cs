using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_CCE_SKILLS_Transaction", Schema = "Exm")]
    public class Exm_CCE_SKILLS_TransactionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public int ECST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int ECS_Id { get; set; }
        public int ECSA_Id { get; set; }
        public int? ECT_Id { get; set; }
        public int? EME_Id { get; set; }
       // public decimal? ECST_Score { get; set; }
        public string ECST_Score { get; set; }
        public int EMGR_Id { get; set; }
        public bool ECST_ActiveFlag { get; set; }
    }
}
