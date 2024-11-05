using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7112_CodeOfCoduct_Files")]
    public class NAAC_AC_7112_CodeOfCoduct_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7112CODCONF_Id { get; set; }
        public long NCAC7112CODCON_Id { get; set; }
        public string NCAC7112CODCONF_FileName { get; set; }
        public string NCAC7112CODCONF_Filedesc { get; set; }
        public string NCAC7112CODCONF_FilePath { get; set; }
        public string NCAC7112CODCONF_StatusFlg { get; set; }
        public bool NCAC7112CODCONF_ActiveFlg { get; set; }
    }
}
