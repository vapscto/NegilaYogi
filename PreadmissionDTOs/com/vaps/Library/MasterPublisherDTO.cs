using System;
using System.Collections.Generic;
using System.Text;
using PreadmissionDTOs;
namespace PreadmissionDTOs.com.vaps.Library
{
    public class MasterPublisherDTO:CommonParamDTO
    {
        public long LMP_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMP_PublisherName { get; set; }
        public long? LMP_MobileNo { get; set; }
        public string LMP_PhoneNo { get; set; }
        public string LMP_EMailId { get; set; }
        public string LMP_Address { get; set; }
        public bool LMP_ActiveFlg { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array pulishlist { get; set; }
    }
}
