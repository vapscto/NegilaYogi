using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_718_WasteManagement_Files")]
    public class NAAC_AC_718_WasteManagement_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC718WAMANF_Id { get; set; }
        public long NCAC718WAMAN_Id { get; set; }
        public string NCAC718WAMANF_FileName { get; set; }
        public string NCAC718WAMANF_Filedesc { get; set; }
        public string NCAC718WAMANF_FilePath { get; set; }
        public string NCAC718WAMANF_StatusFlg { get; set; }
        public bool? NCAC718WAMANF_ActiveFlg { get; set; }
    }
}
