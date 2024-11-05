using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.HRMS
{
    [Table("NAAC_AC_Committee_Members")]
    public class NAACACCommitteeMembersDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACCOMMM_Id { get; set; }
        public long NCACCOMM_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCACCOMMM_MemberName { get; set; }
        public string NCACCOMMM_MemberDetails { get; set; }
        public long NCACCOMMM_MemberPhoneNo { get; set; }
        public string NCACCOMMM_MemberEmailId { get; set; }
        public string NCACCOMMM_Role { get; set; }
        public string NCACCOMMM_FileName { get; set; }
        public string NCACCOMMM_FilePath { get; set; }
        public bool NCACCOMMM_ActiveFlg { get; set; }
        public long NCACCOMMM_CreatedBy { get; set; }
        public long NCACCOMMM_UpdatedBy { get; set; }
        public DateTime? NCACCOMMM_CreatedDate { get; set; }
        public DateTime? NCACCOMMM_UpdatedDate { get; set; }
    }
}
