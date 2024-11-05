using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7116_StatutoryBodies_Files")]
    public class NAAC_AC_7116_StatutoryBodies_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7116STABODF_Id { get; set; }
        public long NCAC7116STABOD_Id { get; set; }
        public string NCAC7116STABODF_FileName { get; set; }
        public string NCAC7116STABODF_Filedesc { get; set; }
        public string NCAC7116STABODF_FilePath { get; set; }
        public string NCAC7116STABODF_StatusFlg { get; set; }
        public bool? NCAC7116STABODF_ActiveFlg { get; set; }
    }
}
