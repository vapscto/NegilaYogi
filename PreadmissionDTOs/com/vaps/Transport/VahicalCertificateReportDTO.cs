using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class VahicalCertificateReportDTO
    {
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



         public string TRMV_VehicleNo { get; set; }
        public Array fillvahicleno { get; set; }
        public Array fillvahicletype { get; set; }
        public Array geteditdata { get; set; }
        public Array modeOfPaymentList { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array getloaddata { get; set; }
        public string TRVCT_VECompanyName { get; set; }
        public string TRVCT_RTOName { get; set; }
        public string TRVCT_InsuranceCompany { get; set; }
        public string TRVCT_PolicyNo { get; set; }
        public string TRMV_CompanyName { get; set; }
        public vehicleid[] vhlid { get; set; }
        public ctype[] ctype { get; set; }
    }
    public class ctype
    {
        public int id { get; set; }
        public string type { get; set; }
    }
}
