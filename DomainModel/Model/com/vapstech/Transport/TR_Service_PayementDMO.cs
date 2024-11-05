using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Service_Payement", Schema = "TRN")]
    public class TR_Service_PayementDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  TRSEP_Id { get; set; }
        public long TRSE_Id { get; set; }
        public string TRSEP_ModeOfPayment { get; set; }
        public string TRSEP_TransactionRefNo { get; set; }
        public string TRSEP_ChequeDDNo { get; set; }
        public DateTime? TRSEP_ChequeDDDate { get; set; }
        public DateTime TRSEP_PaymentDate { get; set; }
        public decimal TRSEP_Amount { get; set; }
        public string TRSEP_BankName { get; set; }
        public bool TRSEP_ActiveFlag { get; set; }
        public long TRSEP_CreatedBy { get; set; }
        public long TRSEP_UpdatedBy { get; set; }
        
    }
}
