using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7113_CoreValues_Files")]
    public class NAAC_AC_7113_CoreValues_FilesDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7113CORVALF_Id { get; set; }
        public long NCAC7113CORVAL_Id { get; set; }
       
        public string NCAC7113CORVALF_FileName { get; set; }
        public string NCAC7113CORVALF_Filedesc { get; set; }
        public string NCAC7113CORVALF_FilePath { get; set; }
        public string NCAC7113CORVALF_StatusFlg { get; set; }
        public bool NCAC7113CORVALF_ActiveFlg { get; set; }
    }
}
