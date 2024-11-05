using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class VahicalCertificateDTO
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
        public Array geteditdata { get; set; }
        public Array modeOfPaymentList { get; set; }
        public Array documentdetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array getloaddata { get; set; }
        public Array mobilenolist { get; set; }
        public Array emailist { get; set; }
        public string TRVCT_VECompanyName { get; set; }
        public string TRVCT_RTOName { get; set; }
        public string TRVCT_InsuranceCompany { get; set; }
        public string TRVCT_PolicyNo { get; set; }
        public Mobile_Number_DTO[] mobile_list_dto { get; set; }

        public Email_Id_DTO[] email_list_dto { get; set; }

        public string TRVCT_SMSAlertToNo { get; set; }
        public string TRVCT_eMailAlertTo { get; set; }
        public uploaddocments[] uploaddocments { get; set; }

    }
    public class uploaddocments
    {
        public string TRVCTD_Id { get; set; }
        public string TRVCTD_FileName { get; set; }
        public string TRVCTD_FileLocation { get; set; }
    }
}
