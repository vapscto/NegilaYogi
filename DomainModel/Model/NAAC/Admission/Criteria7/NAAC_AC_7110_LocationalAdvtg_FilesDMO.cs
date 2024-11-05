using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7110_LocationalAdvtg_Files")]
    public class NAAC_AC_7110_LocationalAdvtg_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7110LOCADVTGF_Id { get; set; }
        public long NCAC7110LOCADVTG_Id { get; set; }
        public string NCAC7110LOCADVTGF_Filedesc { get; set; }
        public string NCAC7110LOCADVTGF_FileName { get; set; }
        public string NCAC7110LOCADVTGF_FilePath { get; set; }
        public string NCAC7110LOCADVTGF_StatusFlg { get; set; }
        public bool NCAC7110LOCADVTGF_ActiveFlg { get; set; }
    }
}
