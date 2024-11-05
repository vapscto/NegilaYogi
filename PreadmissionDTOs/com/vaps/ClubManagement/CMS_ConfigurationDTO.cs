using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
    public class CMS_ConfigurationDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array getreport { get; set; }
        public string returnval { get; set; }
        public long CMSCON_Id { get; set; }
       
        public bool CMSCON_ApplicationApplFlg { get; set; }
        public bool CMSCON_DiscountApplFlg { get; set; }
        public bool CMSCON_BOMFlg { get; set; }
        public bool CMSCON_CategorywiseFlg { get; set; }
        public bool CMSCON_CreditFlg { get; set; }
        public bool CMSCON_IncentiveApplFlg { get; set; }
        public bool CMSCON_TaxApplFlg { get; set; }
        public bool CMSCON_PayLateFeeInterestFlg { get; set; }
        public decimal CMSCON_InterestPercent { get; set; }
        public long CMSCON_MaxNoDependent { get; set; }
        public long CMSCON_NoOfProposal { get; set; }
        public bool CMSCON_AllowNonMemberCreditTransFlg { get; set; }
        public bool CMSCON_CoverChargeAmtFlg { get; set; }
        public bool CMSCON_ActiveFlag { get; set; }
    }
}
