using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7114_HumanValues_File_Comments")]
   public class NAAC_AC_7114_HumanValues_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7114HUVALFC_Id { get; set; }
        public long NCAC7114HUVALFC_RemarksBy { get; set; }
        public long NCAC7114HUVALF_Id { get; set; }
        public string NCAC7114HUVALFC_Remarks { get; set; }
        public string NCAC7114HUVALFC_StatusFlg { get; set; }
        public bool NCAC7114HUVALFC_ActiveFlag { get; set; }
        public long NCAC7114HUVALFC_CreatedBy { get; set; }
        public long NCAC7114HUVALFC_UpdatedBy { get; set; }
        public DateTime? NCAC7114HUVALFC_CreatedDate { get; set; }
        public DateTime? NCAC7114HUVALFC_UpdatedDate { get; set; }
    }
}
