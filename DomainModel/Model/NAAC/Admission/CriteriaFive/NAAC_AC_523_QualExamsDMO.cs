using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_523_QualExams")]
    public class NAAC_AC_523_QualExamsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC523QE_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC523QAMA_Id { get; set; }
        public long NCAC523QE_Year { get; set; }
        public long NCAC523QE_NoOfStudents { get; set; }
        public long NCAC523QE_NoOfStudentsappearing { get; set; }
      
    
        public bool NCAC523QE_ActiveFlg { get; set; }
        public string NCAC523QE_StatusFlg { get; set; }
        public long NCAC523QE_CreatedBy { get; set; }
        public long NCAC523QE_UpdatedBy { get; set; }
        public DateTime NCAC523QE_CreatedDate { get; set; }
        public DateTime NCAC523QE_UpdatedDate { get; set; }
        public List<NAAC_AC_523_QualExamsFilesDMO> NAAC_AC_523_QualExamsFilesDMO { get; set; }

    }
}
