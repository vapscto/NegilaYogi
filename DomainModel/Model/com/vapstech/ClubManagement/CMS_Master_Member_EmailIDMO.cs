using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Member_EmailId", Schema = "CMS")]
    public class CMS_Master_Member_EmailIDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMMEMEID_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSMMEMEID_EmailId { get; set; }
        public bool CMSMMEMEID_DeFaultFlag { get; set; }
        public bool CMSMMEMEID_ActiveFlg { get; set; }
        public DateTime? CMSMMEMEID_CreatedDate { get; set; }
        public long CMSMMEMEID_CreatedBy { get; set; }
        public DateTime? CMSMMEMEID_UpdatedDate { get; set; }
        public long CMSMMEMEID_UpdatedBy { get; set; }
    }
}
