using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class ExpirySettingsDTO
    {
        public long TRC_Id { get; set; }
        public long MI_Id { get; set; }
        public long TRVCT_Id { get; set; }
        public int TRC_DLExpReminderDays { get; set; }
        public int TRC_EmmisionExpMonths { get; set; }
        public int TRC_EmmisionExpDays { get; set; }
        public int TRC_TaxExpMonths { get; set; }
        public int TRC_TaxExpDays { get; set; }
        public int TRC_FitnessExpMonths { get; set; }
        public int TRC_FitnessExpDays { get; set; }
        public int TRC_SpeedControlMonths { get; set; }
        public int TRC_SpeedControlDays { get; set; }
        public int TRC_PermitMonths { get; set; }
        public int TRC_PermitDays { get; set; }
        public int TRC_CeaseFireMonths { get; set; }
        public int TRC_CeaseFireDays { get; set; }
        public int TRC_InsuranceMonths { get; set; }
        public int TRC_InsuranceDays { get; set; }
        public int TRC_GreenTaxMonths { get; set; }
        public int TRC_GreenTaxDays { get; set; }
        public Array getdatadetails { get; set; }
        public Array geteditdataarea { get; set; }
        public string message { get; set; }
        public bool retrval { get; set; }

        public string TRVCT_CertificateType { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? TRVCT_ObtainedDate { get; set; }
        public DateTime? TRVCT_ValidTillDate { get; set; }
        public string TRMV_VehicleName { get; set; }
        public string TRMV_VehicleNo { get; set; }
        public string TRVCT_SMSAlertToNo { get; set; }
        public string TRVCT_eMailAlertTo { get; set; }
        public int remainingdays { get; set; }

        public Array vahicalexpreminder { get; set; }
        public Array vahicalexp { get; set; }
        public Array DLmainreminderlist { get; set; }
        public Array DLmainexpiredlist { get; set; }



    }
}
