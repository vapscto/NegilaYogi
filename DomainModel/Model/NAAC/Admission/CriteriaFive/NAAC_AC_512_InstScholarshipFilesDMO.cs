using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_512_InstScholarship_Files")]
    public class NAAC_AC_512_InstScholarshipFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC512INSCHF_Id { get; set; }
        public long NCAC512INSCH_Id { get; set; }
        public string NCAC512INSCHF_FileName { get; set; }
        public string NCAC512INSCHF_FilePath { get; set; }
        public string NCAC512INSCHF_Filedesc { get; set; }
        public string NCAC512INSCHF_StatusFlg { get; set; }
        public bool? NCAC512INSCHF_ActiveFlg { get; set; }
        

    }
}
