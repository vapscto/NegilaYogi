using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7114_HumanValues_Comments")]
  public  class NAAC_AC_7114_HumanValues_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7114HUVALC_Id { get; set; }
        public long NCAC7114HUVALC_RemarksBy { get; set; }
        public long NCAC7114HUVAL_Id { get; set; }
        public string NCAC7114HUVALC_Remarks { get; set; }
        public string NCAC7114HUVALC_StatusFlg { get; set; }
        public bool NCAC7114HUVALC_ActiveFlag { get; set; }
        public long NCAC7114HUVALC_CreatedBy { get; set; }
        public long NCAC7114HUVALC_UpdatedBy { get; set; }
        public DateTime? NCAC7114HUVALC_CreatedDate { get; set; }
        public DateTime? NCAC7114HUVALC_UpdatedDate { get; set; }
    }
}
