using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7111_LocalCommunity_Comments")]
   public class NAAC_AC_7111_LocalCommunity_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7111LOCCOMC_Id { get; set; }
        public long NCAC7111LOCCOM_Id { get; set; }
        public long NCAC7111LOCCOMC_RemarksBy { get; set; }
        public string NCAC7111LOCCOMC_Remarks { get; set; }
        public string NCAC7111LOCCOMC_StatusFlg { get; set; }
        public bool NCAC7111LOCCOMC_ActiveFlag { get; set; }
        public long NCAC7111LOCCOMC_CreatedBy { get; set; }
        public long NCAC7111LOCCOMC_UpdatedBy { get; set; }
        public DateTime? NCAC7111LOCCOMC_CreatedDate { get; set; }
        public DateTime? NCAC7111LOCCOMC_UpdatedDate { get; set; }
    }
}
