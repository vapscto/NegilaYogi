using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_424_Expenditure_Files")]
    public class NAAC_AC_424_Expenditure_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC424EXPF_Id { get; set; }
        public string NCAC424EXPF_FileName { get; set; }
        public string NCAC424EXPF_FilePath { get; set; }
        public string NCAC424EXPF_Filedesc { get; set; }
        public long NCAC424EXP_Id { get; set; }
        public string NCAC424EXPF_StatusFlg { get; set; }
        public bool NCAC424EXPF_ActiveFlg { get; set; }

    }
}
