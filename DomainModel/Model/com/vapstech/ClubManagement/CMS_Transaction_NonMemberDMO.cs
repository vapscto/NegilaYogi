using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Transaction_NonMember", Schema = "CMS")]
    public class CMS_Transaction_NonMemberDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSTRANSNMEM_Id { get; set; }
        public long CMSTRANS_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public string CMSTRANSNMEM_NonMemberName { get; set; }
        public string CMSTRANSNMEM_ContactNo { get; set; }
        public string CMSTRANSNMEM_EmailId { get; set; }
        public string CMSTRANSNMEM_Address { get; set; }
        public bool CMSTRANNSMEM_ActiveFlg { get; set; }
        public DateTime? CMSTRANSNMEM_CreatedDate { get; set; }
        public long CMSTRANSNMEM_CreatedBy { get; set; }
        public DateTime? CMSTRANSNMEM_UpdatedDate { get; set; }
        public long CMSTRANSNMEM_UpdatedBy { get; set; }

    }
}
