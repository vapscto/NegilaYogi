using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Y_Payment_OthStu")]
    public class Fee_Y_Payment_OthStuDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FYPOST_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long FYP_Id { get; set; }
        public long FMOST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FYPOST_TotalPaidAmount { get; set; }
        public long FYPOST_TotalWaivedAmount { get; set; }
        public long FYPOST_TotalConcessionAmount { get; set; }
        public decimal FYPOST_TotalFineAmount { get; set; }
       
    }
}
