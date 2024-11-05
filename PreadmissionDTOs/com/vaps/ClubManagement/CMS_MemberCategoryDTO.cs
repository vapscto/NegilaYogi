using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_MemberCategoryDTO
    {
        public long CMSMCAT_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string CMSMCAT_CategoryName { get; set; }
        public string CMSMCAT_CategoryCode { get; set; }
        public string CMSMCAT_AllowCreditTransFlg { get; set; }
        public decimal CMSMCAT_MaxCreditLimit { get; set; }
        public long CMSMCAT_MaxNoOfGuest { get; set; }
        public bool CMSMCAT_EligibleForProposerFlg { get; set; }
        public bool CMSMCAT_MinTransApplFlg { get; set; }
        public decimal CMSMCAT_MinTransAmt { get; set; }
        public bool CMSMCAT_AllowBlockFlg { get; set; }
        public bool CMSMCAT_AllowTerminateFlg { get; set; }
        public bool CMSMCAT_PayLateFeeInterestFlg { get; set; }
        public bool CMSMCAT_TakeCompulsoryServicesFlg { get; set; }
        public long CMSMCAT_MaxNoOfDependents { get; set; }
        public bool CMSMCAT_MembershipExpiryFlg { get; set; }
        public string CMSMCAT_MembershipDuration { get; set; }
       
        public bool CMSMCAT_ActiveFlag { get; set; }
        public Array getreport { get; set; }
        public Array edit { get; set; }
        public string returnval { get; set; }

    }
}
