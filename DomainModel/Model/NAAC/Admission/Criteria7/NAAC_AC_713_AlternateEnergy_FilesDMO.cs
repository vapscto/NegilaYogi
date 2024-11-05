using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_713_AlternateEnergy_Files")]
    public class NAAC_AC_713_AlternateEnergy_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC713ALTENEF_Id { get; set; }
        public long NCAC713ALTENE_Id { get; set; }
        public string NCAC713ALTENEF_FileName { get; set; }
        public string NCAC713ALTENEF_Filedesc { get; set; }
        public string NCAC713ALTENEF_FilePath { get; set; }
    }
}
