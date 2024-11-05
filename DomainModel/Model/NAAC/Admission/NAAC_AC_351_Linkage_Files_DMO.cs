using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_351_Linkage_Files")]
  public class NAAC_AC_351_Linkage_Files_DMO
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCAC351LINF_Id { get; set; }
      public long NCAC351LIN_Id { get; set; }
      public string NCAC351LINF_FileName { get; set; }
      public string NCAC351LINF_Filedesc { get; set; }
      public string NCAC351LINF_FilePath { get; set; }
      public string NCAC351LINF_StatusFlg { get; set; }
      public bool NCAC351LINF_ActiveFlg { get; set; }
    }
}
