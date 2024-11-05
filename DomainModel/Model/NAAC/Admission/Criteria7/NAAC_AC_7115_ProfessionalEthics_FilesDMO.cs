using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7115_ProfessionalEthics_Files")]
    public class NAAC_AC_7115_ProfessionalEthics_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7115PROETHF_Id { get; set; }
        public long NCAC7115PROETH_Id { get; set; }
        public string NCAC7115PROETHF_FileName { get; set; }
        public string NCAC7115PROETHF_Filedesc { get; set; }
        public string NCAC7115PROETHF_FilePath { get; set; }
        public string NCAC7115PROETHF_StatusFlg { get; set; }
        public bool NCAC7115PROETHF_ActiveFlg { get; set; }
    }
}
