using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7117_UniversalValues_Files")]
    public class NAAC_AC_7117_UniversalValues_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7117UNIVALF_Id { get; set; }
        public long NCAC7117UNIVAL_Id { get; set; }
        public string NCAC7117UNIVALF_Filedesc { get; set; }
        public string NCAC7117UNIVALF_FileName { get; set; }
        public string NCAC7117UNIVALF_FilePath { get; set; }
        public string NCAC7117UNIVALF_StatusFlg { get; set; }
        public bool NCAC7117UNIVALF_ActiveFlg { get; set; }
    }
}
