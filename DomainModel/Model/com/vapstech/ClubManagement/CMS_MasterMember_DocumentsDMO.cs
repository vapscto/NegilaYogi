using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_Member_Documents", Schema = "CMS")]
    public class CMS_MasterMember_DocumentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMMEMDOC_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public  string CMSMMEMDOC_DocumentName { get; set; }
        public string CMSMMEMDOC_FileName { get; set; }
        public string CMSMMEMDOC_FilePath { get; set; }
        public bool CMSMMEMDOC_ActiveFlg { get; set; }
        public DateTime? CMSMMEMDOC_CreatedDate { get; set; }
        public long CMSMMEMDOC_CreatedBy { get; set; }
        public DateTime? CMSMMEMDOC_UpdatedDate { get; set; }
        public long CMSMMEMDOC_UpdatedBy { get; set; }
    }
}
