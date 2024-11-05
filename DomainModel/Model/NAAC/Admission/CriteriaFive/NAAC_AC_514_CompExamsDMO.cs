using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_514_CompExams")]
    public class NAAC_AC_514_CompExamsDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
    public long NCAC514CPEX_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC514CPEX_ImpYear { get; set; }
        public string NCAC514CPEX_ExamSchemeName { get; set; }
        public long NCAC514CPEX_NoOfStudents { get; set; }
   
        public bool NCAC514CPEX_ActiveFlg { get; set; }
        public long NCAC514CPEX_CreatedBy { get; set; }
        public long NCAC514CPEX_UpdatedBy { get; set; }
        public string NCAC514CPEX_StatusFlg { get; set; }
        public DateTime NCAC514CPEX_CreatedDate { get; set; }
        public DateTime NCAC514CPEX_UpdatedDate { get; set; }

        public List<NAAC_AC_514_CompExamsFilesDMO> NAAC_AC_514_CompExamsFilesDMO { get; set; }

    }
}
