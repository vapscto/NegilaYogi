using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Member_Blocked", Schema = "CMS")]
    public class CMS_Master_MemberBlockedDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CMSMMEMBLK_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public DateTime? CMSMMEMBLK_BlockedDate { get; set; }
        public string CMSMMEMBLK_ReasonForBlock { get; set; }
        public DateTime? CMSMMEMBLK_RenewalDate { get; set; }
        public bool CMSMMEMBLK_ActiveFlg { get; set; }
        public DateTime? CMSMMEMBLK_CreatedDate { get; set; }
        public long CMSMMEMBLK_CreatedBy { get; set; }
        public DateTime? CMSMMEMBLK_UpdatedDate { get; set; }
        public long CMSMMEMBLK_UpdatedBy { get; set; }
    }
}
