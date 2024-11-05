using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Transaction_Member", Schema = "CMS")]

    public class CMS_Transaction_MemberDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSMEM_Id { get; set; }
        public long CMSTRANS_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public bool CMSTRANSMEM_ActiveFlg { get; set; }
        public DateTime? CMSTRANSMEM_CreatedDate { get; set; }
        public long CMSTRANSMEM_CreatedBy { get; set; }
        public DateTime? CMSTRANSMEM_UpdatedDate { get; set; }
        public long CMSTRANSMEM_UpdatedBy { get; set; }
        
    }
}
