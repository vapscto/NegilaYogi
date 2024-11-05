using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_514_CompExams_Files")]
    public class NAAC_AC_514_CompExamsFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
       public long NCAC514CPEXF_Id { get; set; }
       public long NCAC514CPEX_Id { get; set; }
        public string NCAC514CPEXF_FileName { get; set; }
        public string NCAC514CPEXF_Filedesc { get; set; }
        public string NCAC514CPEXF_FilePath { get; set; }
        public string NCAC514CPEXF_StatusFlg { get; set; }
        public bool? NCAC514CPEXF_ActiveFlg { get; set; }
        

    }
}
