using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Member_MobileNo", Schema = "CMS")]
    public class CMS_Master_MemberMobileNoDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMMEMMN_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public long CMSMMEMMN_MobileNo { get; set; }
        public bool CMSMMEMMN_DeFaultFlag { get; set; }
        public bool CMSMMEMMN_ActiveFlg { get; set; }
        public DateTime? CMSMMEMMN_CreatedDate { get; set; }
        public long CMSMMEMMN_CreatedBy { get; set; }
        public DateTime? CMSMMEMMN_UpdatedDate { get; set; }
        public long CMSMMEMMN_UpdatedBy { get; set; }

    }
}
