using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_513_DevSchemes_Files")]
    public class NAAC_AC_513_DevSchemeFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC513INSCHF_Id { get; set; }
        public long NCAC513INSCH_Id { get; set; }
       
        public string NCAC513INSCHF_FileName { get; set; }
        public string NCAC513INSCHF_FilePath { get; set; }
        public string NCAC513INSCHF_Filedesc { get; set; }
        public string NCAC513INSCHF_StatusFlg { get; set; }
        public bool? NCAC513INSCHF_ActiveFlg { get; set; }

        

    }
}
