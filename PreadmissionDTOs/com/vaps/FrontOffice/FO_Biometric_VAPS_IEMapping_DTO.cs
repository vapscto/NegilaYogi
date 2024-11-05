using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class FO_Biometric_VAPS_IEMapping_DTO
    {
        public long FOBVIEM_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOBVIEM_BiometricId { get; set; }
        public long FOBVIEM_HRMEId { get; set; }
        public string FOBVIEM_InsertURL { get; set; }
        public bool FOBVIEM_ActiveFlg { get; set; }
        public long FOBVIEM_Insert_MI_Id { get; set; }
        public string FOBVIEM_CreatedBy { get; set; }
        public string FOBVIEM_UpdatedBy { get; set; }
        public Array filltypes { get; set; }
        public FO_Biometric_VAPS_IEMapping_DTO data { get; set; }
        public string HRME_BiometricCode { get; set; }
    }
}
