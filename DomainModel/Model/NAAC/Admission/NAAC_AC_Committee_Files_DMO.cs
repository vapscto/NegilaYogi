using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_Files")]
   public class NAAC_AC_Committee_Files_DMO
    {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCACCOMMF_Id { get; set; }
      public long NCACCOMM_Id { get; set; }
      public string NCACCOMMF_FileName { get; set; }
      public string NCACCOMMF_FileDesc { get; set; }
      public string NCACCOMMF_FilePath { get; set; }
        public string NCACCOMMF_StatusFlg { get; set; }
        public bool? NCACCOMMF_ActiveFlg { get; set; }
    }
}
