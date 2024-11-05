using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Trip_Payment",Schema ="TRN")]
    public class TR_Trip_PaymentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRTPP_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRTOB_Id { get; set; }
        public DateTime? TRTPP_ReceiptDate { get; set; }
        public string TRTPP_ReceiptNo { get; set; }
       
        public decimal TRTPP_PaidAmount { get; set; }
       
        public string TRTPP_PaymentMode { get; set; }
        public string TRTPP_ReceiptReferenceNo { get; set; }
        public long TRTPP_ChequeDDNo { get; set; }
        public DateTime? TRTPP_ChequeDDDate { get; set; }
        public bool TRTPP_ActiveFlag { get; set; }
        public string TRTPP_BankName { get; set; }
        public TR_Trip_Payment_TripsDMO TR_Trip_Payment_TripsDMO { get; set; }

    }
}
