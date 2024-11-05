using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_642_Funds_files")]
    public class NAAC_AC_642_Funds_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC642FUNDF_Id { get; set; }
        [ForeignKey("NCAC642FUND_Id")]
        public long NCAC642FUND_Id { get; set; }
        public string NCAC642FUNDF_FileName { get; set; }
        public string NCAC642FUNDF_Filedesc { get; set; }
        public string NCAC642FUNDF_FilePath { get; set; }
        public string NCAC642FUNDF_StatusFlg { get; set; }
        public bool? NCAC642FUNDF_ActiveFlg { get; set; }
        public bool? NCAC642FUNDF_ApprovedFlg { get; set; }
        public string NCAC642FUNDF_Remarks { get; set; }


    }
}
