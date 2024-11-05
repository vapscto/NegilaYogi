
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Student_Rebate")]
    public class FeeStudentRebateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FSREB_Id { get; set; }
        public long MI_Id { get; set; }
        public long FYP_Id { get; set; }
        public long AMST_Id { get; set; }
        public long FMA_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FSREB_Date { get; set; }
        public long FMH_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FTI_Id { get; set; }
        public decimal FSREB_RebateAmount { get; set; }
        public string FSREB_Remarks { get; set; }
        public bool FSREB_ActiveFlag { get; set; }
    }
}
