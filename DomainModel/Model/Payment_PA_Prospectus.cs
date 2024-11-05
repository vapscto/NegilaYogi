using DomainModel.Model.com.vaps.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Fee_Y_Payment_PA_Prospectus")]
    public class Payment_PA_Prospectus : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FYPPP_Id { get; set; }
        public long FYP_Id { get; set; }
        [ForeignKey("FYP_Id")]
        public long PASP_Id { get; set; }
        [ForeignKey("PASP_Id")]
        public decimal FYPPP_TotalPaidAmount { get; set; }
      
        public bool FYPPP_ActiveFlag { get; set; }

        public FeePaymentDetailsDMO feePaymentDetails { get; set; }


    }
}
