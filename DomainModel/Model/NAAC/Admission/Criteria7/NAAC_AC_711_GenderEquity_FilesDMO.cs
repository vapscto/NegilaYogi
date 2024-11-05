using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_711_GenderEquity_Files")]
    public class NAAC_AC_711_GenderEquity_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC711GENEQF_Id { get; set; }
        public string NCAC711GENEQF_Filedesc { get; set; }
        public string NCAC711GENEQF_FileName { get; set; }
        public string NCAC711GENEQF_FilePath { get; set; }
        public string NCAC711GENEQF_StatusFlg { get; set; }
        public long NCAC711GENEQ_Id { get; set; }
        public bool NCAC711GENEQF_ActiveFlg { get; set; }
    }
}
