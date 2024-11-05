using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeCancellationConfigurationDTO
    {
        public long MI_Id { get; set; }
        public long ACACC_Id { get; set; }
        public int ACACC_DOAFlg { get; set; }
        public int ACACC_FromDays { get; set; }
        public int ACACC_ToDays { get; set; }
        public decimal ACACC_RefundAmountPer { get; set; }
        public decimal ACACC_CancellationPer { get; set; }
        public bool ACACC_ActiveFlag { get; set; }
        public long ACACC_CreatedBy { get; set; }
        public long ACACC_UpdatedBy { get; set; }
        public Array getdetails { get; set; }
        public Array editdetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }

    }
}
