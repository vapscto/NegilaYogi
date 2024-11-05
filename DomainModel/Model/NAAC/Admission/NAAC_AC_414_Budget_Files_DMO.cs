using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_414_Budget_Files")]
    public class NAAC_AC_414_Budget_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC414BDF_Id { get; set; }
        public string NCAC414BDF_FileName { get; set; }
        public string NCAC414BDF_Filedesc { get; set; }
        public string NCAC414BDF_FilePath { get; set; }
        public string NCAC414BDF_StatusFlg { get; set; }
        public bool NCAC414BDF_ActiveFlg { get; set; }
        public long NCAC414BD_Id { get; set; }

    }
}
