using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_VAC_132_Files")]
   public class NAAC_AC_VAC_132_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCACVAC132F_Id { get; set;}
        public long NCACVAC132_Id { get; set; }
        public string NCACVAC132F_FileName { get; set; }
        public string NCACVAC132F_FilePath { get; set; }
        public string NCACVAC132F_Filedesc { get; set; }
        public string NCACVAC132F_StatusFlg { get; set; }
        public bool? NCACVAC132F_ActiveFlg { get; set; }
    }
}
