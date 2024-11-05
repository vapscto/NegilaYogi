using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.PDA
{
    [Table("PDA_Status")]
    public class PDA_StatusDMO : CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PDAS_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public decimal PDAS_OBStudentDue { get; set; }
        public decimal PDAS_OBExcessPaid { get; set; }
        public decimal PDAS_CYDeposit { get; set; }
        public decimal PDAS_CYExpenses { get; set; }
        public decimal PDAS_CYRefundAmt { get; set; }
        public decimal PDAS_CBStudentDue { get; set; }
        public decimal PDAS_CBExcessPaid { get; set; }
      
    }
}
