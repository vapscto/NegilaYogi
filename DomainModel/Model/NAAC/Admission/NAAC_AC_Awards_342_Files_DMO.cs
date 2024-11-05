using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Awards_342_Files")]
  public  class NAAC_AC_Awards_342_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCACAW342F_Id { get; set; }
      public long NCACAW342_Id { get; set; }
      public string NCACAW342F_FileName { get; set; }
      public string NCACAW342F_Filedesc { get; set; }
      public string NCACAW342F_FilePath { get; set; }
      public string NCACAW342F_StatusFlg { get; set; }
      public bool? NCACAW342F_ActiveFlg { get; set; }

    }
}
