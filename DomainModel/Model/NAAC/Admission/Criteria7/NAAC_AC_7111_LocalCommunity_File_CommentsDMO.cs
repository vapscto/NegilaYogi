using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7111_LocalCommunity_File_Comments")]
    public class NAAC_AC_7111_LocalCommunity_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7111LOCCOMFC_Id { get; set; }
        public long NCAC7111LOCCOMF_Id { get; set; }
        public long NCAC7111LOCCOMFC_RemarksBy { get; set; }
        public string NCAC7111LOCCOMFC_Remarks { get; set; }
        public string NCAC7111LOCCOMFC_StatusFlg { get; set; }
        public bool NCAC7111LOCCOMFC_ActiveFlag { get; set; }
        public long NCAC7111LOCCOMFC_CreatedBy { get; set; }
        public long NCAC7111LOCCOMFC_UpdatedBy { get; set; }
        public DateTime? NCAC7111LOCCOMFC_CreatedDate { get; set; }
        public DateTime? NCAC7111LOCCOMFC_UpdatedDate { get; set; }
    }
}
