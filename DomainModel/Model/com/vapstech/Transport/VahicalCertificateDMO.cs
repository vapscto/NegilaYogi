using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Vehicle_Certificates", Schema = "TRN")]
    public class VahicalCertificateDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVCT_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? TRVCT_ObtainedDate { get; set; }
        public DateTime? TRVCT_ValidTillDate { get; set; }
        public decimal TRVCT_AmountPaid { get; set; }
        public string TRVCT_ModeOfPayment { get; set; }
        public string TRVCT_Remarks { get; set; }
        public long TRVCT_PaymentReferenceNo { get; set; }
        public long TRVCT_ChequeDDNo { get; set; }
        public DateTime? TRVCT_ChequeDDDate { get; set; }
        public string TRVCT_CertificateType { get; set; }
        public string TRVCT_VECompanyName { get; set; }
        public string TRVCT_RTOName { get; set; }
        public string TRVCT_InsuranceCompany { get; set; }
        public string TRVCT_PolicyNo { get; set; }
        public string TRVCT_SMSAlertToNo { get; set; }
        public string TRVCT_eMailAlertTo { get; set; }



    }
}
