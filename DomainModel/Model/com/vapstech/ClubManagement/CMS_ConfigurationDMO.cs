﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Configuration", Schema = "CMS")]
    public class CMS_ConfigurationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSCON_Id { get; set; }
        public long MI_Id { get; set; }
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
        public DateTime? CMSCON_CreatedDate { get; set; }
        public long CMSCON_CreatedBy { get; set; }
        public DateTime? CMSCON_UpdatedDate { get; set; }
        public long CMSCON_UpdatedBy { get; set; }
    }
}
