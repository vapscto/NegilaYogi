using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_523_QualExams_Files")]
    public class NAAC_AC_523_QualExamsFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC523QEF_Id { get; set; }
        public long NCAC523QE_Id { get; set; }
        public string NCAC523QEF_FileName { get; set; }
        public string NCAC523QEF_Filedesc { get; set; }
        public string NCAC523QEF_FilePath { get; set; }
        public string NCAC523QEF_StatusFlg { get; set; }
        public bool? NCAC523QEF_ActiveFlg { get; set; }

        

    }
}
