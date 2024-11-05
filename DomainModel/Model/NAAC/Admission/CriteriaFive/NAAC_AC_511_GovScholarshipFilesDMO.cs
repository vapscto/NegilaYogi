using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_511_GovScholarship_Files")]
    public class NAAC_AC_511_GovScholarshipFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC511GSCHF_Id { get; set; }
        public long NCAC511GSCH_Id { get; set; }
      
        public string NCAC511GSCHF_FileName { get; set; }
        public string NCAC511GSCHF_FilePath { get; set; }
        public string NCAC511GSCHF_Filedesc { get; set; }
        public string NCAC511GSCHF_StatusFlg { get; set; }
        public bool? NCAC511GSCHF_ActiveFlg { get; set; }


    }
}
