using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_714_LEDBulbs_Files")]
    public class NAAC_AC_714_LEDBulbs_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC714LEDBUF_Id { get; set; }
        public long NCAC714LEDBU_Id { get; set; }
        public string NCAC714LEDBUF_FileName { get; set; }
        public string NCAC714LEDBUF_Filedesc { get; set; }
        public string NCAC714LEDBUF_FilePath { get; set; }
        public string NCAC714LEDBUF_StatusFlg { get; set; }
        public bool? NCAC714LEDBUF_ActiveFlg { get; set; }
    }
}
