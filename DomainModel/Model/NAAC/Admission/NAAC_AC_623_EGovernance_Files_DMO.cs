using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_623_EGovernance_Files")]
    public class NAAC_AC_623_EGovernance_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC623EGOVF_Id { get; set; }
        public long NCAC623EGOV_Id { get; set; }
        public string NCAC623EGOVF_FileName { get; set; }
        public string NCAC623EGOVF_Filedesc { get; set; }
        public string NCAC623EGOVF_FilePath { get; set; }
        public string NCAC623EGOVF_StatusFlg { get; set; }
        public bool? NCAC623EGOVF_ActiveFlg { get; set; }
        public bool? NCAC623EGOVF_ApprovedFlg { get; set; }
        public string NCAC623EGOVF_Remarks { get; set; }

    }
}
