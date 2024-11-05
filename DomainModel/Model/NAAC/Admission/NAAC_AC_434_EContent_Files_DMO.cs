using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_434_EContent_Files")]
    public class NAAC_AC_434_EContent_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC434ECTF_Id { get; set; }
        public string NCAC434ECTF_FileName { get; set; }
        public string NCAC434ECTF_Filedesc { get; set; }
        public string NCAC434ECTF_FilePath { get; set; }
        public long NCAC434ECT_Id { get; set; }
        public string NCAC434ECTF_StatusFlg { get; set; }
        public bool NCAC434ECTF_ActiveFlg { get; set; }
    }
}
