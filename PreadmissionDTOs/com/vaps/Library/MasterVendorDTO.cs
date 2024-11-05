using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class MasterVendorDTO:CommonParamDTO
    {
        public long LMV_Id { get; set; }
        public long MI_Id { get; set; }
        public long? LMV_MobileNo { get; set; }
        public string LMV_VendorName { get; set; }
        public string LMV_Address { get; set; }
        public string LMV_EMailId { get; set; }
        public string LMV_PhoneNo { get; set; }
        public bool LMV_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array pulishlist { get; set; }
    }
}
