using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_MembershipApplicationDTO
    {
        public long CMSMAPPL_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array getarray { get; set; }
        public string CMSMAPPL_ApplicantsName { get; set; }
        public string CMSMAPPL_Address { get; set; }
        public string CMSMAPPL_PhoneNo { get; set; }
        public string CMSMAPPL_EMailId { get; set; }
        public DateTime? CMSMAPPL_ApplicationDate { get; set; }
        public string CMSMAPPL_ApplicationNo { get; set; }
        public string CMSMAPPL_ApplicationStatus { get; set; }
        public string CMSMAPPL_ReferredBy { get; set; }
        public bool CMSMAPPL_ApplCancelledFlg { get; set; }
        public DateTime? CMSMAPPL_ApplCancelledDate { get; set; }
        public string CMSMAPPL_ApplCancelledReason { get; set; }
        public bool CMSMAPPL_ActiveFlag { get; set; }

    }
}
