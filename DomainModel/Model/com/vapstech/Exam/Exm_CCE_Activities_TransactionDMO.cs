using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_CCE_Activities_Transaction", Schema = "Exm")]
    public class Exm_CCE_Activities_TransactionDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ECSACTT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int ECACT_Id { get; set; }
        public int ECACTA_Id { get; set; }
        public int? ECT_Id { get; set; }
        public int? EME_Id { get; set; }
       // public decimal? ECSACTT_Score { get; set; }
        //ECSACTT_Score
        public string ECSACTT_Score { get; set; }
        public int EMGR_Id { get; set; }
        public bool ECSACTT_ActiveFlag { get; set; }
    }
}