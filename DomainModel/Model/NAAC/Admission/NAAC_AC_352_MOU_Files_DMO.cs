using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_352_MOU_Files")]
  public class NAAC_AC_352_MOU_Files_DMO
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCAC352MOUF_Id { get; set; }
      public string NCAC352MOUF_FileName { get; set; }
      public string NCAC352MOUF_Filedesc { get; set; }
      public string NCAC352MOUF_FilePath { get; set; }
      public long NCAC352MOU_Id { get; set; }
      public string NCAC352MOUF_StatusFlg { get; set; }
      public bool NCAC352MOUF_ActiveFlg { get; set; }
    }
}
