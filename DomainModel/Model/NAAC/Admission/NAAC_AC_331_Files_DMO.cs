using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_331_Files")]
   public class NAAC_AC_331_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCAC331F_Id { get; set; }
      public long NCAC331_Id { get; set; }
      public string NCAC331F_FileName { get; set; }
      public string NCAC331F_Filedesc { get; set; }
      public string NCAC331F_FilePath { get; set; }
      public string NCAC331F_StatusFlg { get; set; }
      public bool? NCAC331F_ActiveFlg { get; set; }
    }
}
