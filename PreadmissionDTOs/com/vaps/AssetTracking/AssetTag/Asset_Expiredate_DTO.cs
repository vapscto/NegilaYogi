using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.AssetTracking.AssetTag
{
    public class Asset_Expiredate_DTO
    {
        public long user_id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_AppDownloadedDeviceId { get; set; }
        public DateTime? INVAAT_WarantyExpiryDate { get; set; }
        public string INVMS_StoreName { get; set; }
        public string INVMI_ItemName { get; set; }
        public string employeename { get; set; }
        public long HRME_MobileNo { get; set; }

    }
}
