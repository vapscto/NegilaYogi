using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_Members")]
    public class NAAC_AC__Committee_Members_DMO
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
        public bool NCACCOMMM_ActiveFlg { get; set; }
        public long NCACCOMMM_CreatedBy { get; set; }
        public long NCACCOMMM_UpdatedBy { get; set; }
        public DateTime NCACCOMMM_CreatedDate { get; set; }
        public DateTime NCACCOMMM_UpdatedDate { get; set; }
        public string NCACCOMMM_StatusFlg { get; set; }
        public List<NAAC_AC_Committee_Members_files_DMO> NAAC_AC_Committee_Members_files_DMO { get; set; }
    }
}
