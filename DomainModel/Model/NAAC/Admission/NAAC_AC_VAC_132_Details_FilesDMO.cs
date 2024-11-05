using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Details_Files")]
  public  class NAAC_AC_VAC_132_Details_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACVAC132DF_Id { get; set; }
        public long NCACVAC132D_Id { get; set; }
        public string NCACVAC132DF_FileName { get; set; }
        public string NCACVAC132DF_FilePath { get; set; }
        public string NCACVAC132DF_Filedesc { get; set; }
        public string NCACVAC132DF_StatusFlg { get; set; }
        public bool? NCACVAC132DF_ActiveFlg { get; set; }
    }
}
