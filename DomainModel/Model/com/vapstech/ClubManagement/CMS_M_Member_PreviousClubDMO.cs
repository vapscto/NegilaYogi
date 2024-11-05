using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Installments", Schema = "CMS")]
    class CMS_M_Member_PreviousClubDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMMEMPRCLB_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSMMEMPRCLB_ClubName { get; set; }
        public string CMSMMEMPRCLB_ClubAddress { get; set; }
        public string CMSMMEMPRCLB_MembershipType { get; set; }
        public string CMSMMEMPRCLB_ClubPhoneNo { get; set; }
        public string CMSMMEMPRCLB_MembershipNo { get; set; }
        public DateTime? CMSMMEMPRCLB_MemberFromDate { get; set; }
        public DateTime? CMSMMEMPRCLB_MemberToDate { get; set; }
        public bool CMSMMEMPRCLB_ActiveFlg { get; set; }
        public DateTime? CMSMMEMPRCLB_CreatedDate { get; set; }
        public long CMSMMEMPRCLB_CreatedBy { get; set; }
        public DateTime? CMSMMEMPRCLB_UpdatedDate { get; set; }
        public long CMSMMEMPRCLB_UpdatedBy { get; set; }
    }
}
