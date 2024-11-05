using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_512_InstScholarship_Comments")]
    public class NAAC_AC_512_InstScholarship_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC512INSCHC_Id { get; set; }
         public string NCAC512INSCHC_Remarks { get; set; }
        public long NCAC512INSCHC_RemarksBy { get; set; }
        public string NCAC512INSCHC_StatusFlg { get; set; }
        public bool NCAC512INSCHC_ActiveFlag { get; set; }
        public long NCAC512INSCHC_CreatedBy { get; set; }
        public DateTime NCAC512INSCHC_CreatedDate { get; set; }
        public long NCAC512INSCHC_UpdatedBy { get; set; }
        public DateTime NCAC512INSCHC_UpdatedDate { get; set; }
        public long NCAC512INSCH_Id { get; set; }

    }
}
