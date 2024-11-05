using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_M_Member_OtherClubs", Schema = "CMS")]
    public  class CMS_M_MemberOtherClubsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMSMMEMOTHCLB_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSMMEMOTHCLB_ClubName { get; set; }
        public string CMSMMEMOTHCLB_ClubAddress { get; set; }
        public string CMSMMEMOTHCLB_ClubPhoneNo { get; set; }
        public string CMSMMEMOTHCLB_MembershipType { get; set; }
        public string CMSMMEMOTHCLB_MembershipNo { get; set; }
        public DateTime? CMSMMEMOTHCLB_MemberFromDate { get; set; }
        public DateTime? CMSMMEMOTHCLB_MemberExpiryDate { get; set; }
        public bool CMSMMEMOTHCLB_ActiveFlg { get; set; }
        public DateTime? CMSMMEMOTHCLB_CreatedDate { get; set; }
        public long CMSMMEMOTHCLB_CreatedBy { get; set; }
        public DateTime? CMSMMEMOTHCLB_UpdatedDate { get; set; }
        public long CMSMMEMOTHCLB_UpdatedBy { get; set; }
    }
}
