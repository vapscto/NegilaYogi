using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Member_Category", Schema = "CMS")]
    public class CMS_Member_CategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMCAT_Id { get; set; }
        public long MI_Id { get; set; }
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
        public DateTime? CMSMCAT_CreatedDate { get; set; }
        public long CMSMCAT_CreatedBy { get; set; }
        public DateTime? CMSMCAT_UpdatedDate { get; set; }
        public long CMSMCAT_UpdatedBy { get; set; }


    }
}
