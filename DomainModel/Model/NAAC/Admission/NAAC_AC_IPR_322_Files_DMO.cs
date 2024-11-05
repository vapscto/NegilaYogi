using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_IPR_322_Files")]
   public class NAAC_AC_IPR_322_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCACIPR322F_Id { get; set; }
      public long NCACIPR322_Id { get; set; }
      public string NCACIPR322F_FileName { get; set; }
      public string NCACIPR322F_Filedesc { get; set; }
      public string NCACIPR322F_FilePath { get; set; }
      public string NCACIPR322F_StatusFlg { get; set; }
      public bool NCACIPR322F_ActiveFlg { get; set; }
    }
}