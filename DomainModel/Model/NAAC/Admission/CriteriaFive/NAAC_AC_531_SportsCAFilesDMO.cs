using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_531_SportsCA_Files")]
    public class NAAC_AC_531_SportsCAFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC531SPCAF_Id { get; set; }
        public long NCAC531SPCA_Id { get; set; }
        public string NCAC531SPCAF_FileName { get; set; }
        public string NCAC531SPCAF_Filedesc { get; set; }
        public string NCAC531SPCAF_FilePath { get; set; }
        public string NCAC531SPCAF_StatusFlg { get; set; }
        public bool? NCAC531SPCAF_ActiveFlg { get; set; }
        

    }
}
