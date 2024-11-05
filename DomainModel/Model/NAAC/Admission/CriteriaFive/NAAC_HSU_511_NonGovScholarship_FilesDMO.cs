using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_HSU_511_NonGovScholarship_Files")]
    public class NAAC_HSU_511_NonGovScholarship_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC512NGSCHF_Id { get; set; }
        public string NCAC512NGSCHF_FileName { get; set; }
        public string  NCAC512NGSCHF_Filedesc { get; set; }
        public string  NCAC512NGSCHF_FilePath { get; set; }
        public string NCAC512NGSCHF_StatusFlg { get; set; }
        public bool? NCAC512NGSCHF_ActiveFlg { get; set; }
        public long NCAC512NGSCH_Id { get; set; }
    }
}
