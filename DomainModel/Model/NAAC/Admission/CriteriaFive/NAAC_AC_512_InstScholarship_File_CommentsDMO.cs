using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_512_InstScholarship_File_Comments")]
    public class NAAC_AC_512_InstScholarship_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC512INSCHFC_Id { get; set; }
       public string  NCAC512INSCHFC_Remarks { get; set; }
        public long  NCAC512INSCHFC_RemarksBy { get; set; }
        public bool  NCAC512INSCHFC_ActiveFlag { get; set; }
        public long  NCAC512INSCHFC_CreatedBy { get; set; }
        public DateTime  NCAC512INSCHFC_CreatedDate { get; set; }
        public long  NCAC512INSCHFC_UpdatedBy { get; set; }
        public DateTime  NCAC512INSCHFC_UpdatedDate { get; set; }
        public string  NCAC512INSCHFC_StatusFlg { get; set; }
        public long NCAC512INSCHF_Id { get; set; }

    }
}
