using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_MembershipApplication", Schema = "CMS")]
    public class CMS_MembershipApplicationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMAPPL_Id { get; set; }
        public long MI_Id { get; set; }
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
        public DateTime? CMSMAPPL_CreatedDate { get; set; }
        public long CMSMAPPL_CreatedBy { get; set; }
        public DateTime? CMSMAPPL_UpdatedDate { get; set; }
        public long CMSMAPPL_UpdatedBy { get; set; }


    }
}
