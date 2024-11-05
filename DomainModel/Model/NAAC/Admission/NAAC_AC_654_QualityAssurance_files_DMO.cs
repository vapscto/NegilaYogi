using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_654_QualityAssurance_files")]
    public class NAAC_AC_654_QualityAssurance_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC654QUASF_Id { get; set; }
        public long NCAC654QUAS_Id { get; set; }
        public string NCAC654QUASF_FileName { get; set; }
        public string NCAC654QUASF_Filedesc { get; set; }
        public string NCAC654QUASF_FilePath { get; set; }
        public string NCAC654QUASF_StatusFlg { get; set; }
        public bool? NCAC653IQACF_ActiveFlg { get; set; }
        public bool? NCAC654QUASF_ApprovedFlg { get; set; }
        public string NCAC654QUASF_Remarks { get; set; }

    }
}
