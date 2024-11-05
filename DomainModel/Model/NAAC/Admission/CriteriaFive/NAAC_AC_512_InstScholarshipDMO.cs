using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_512_InstScholarship")]
    public class NAAC_AC_512_InstScholarshipDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
       public long NCAC512INSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC512INSCH_Year { get; set; }
        public string NCAC512INSCH_SchemeName { get; set; }
        public long NCAC512INSCH_NoOfStudents { get; set; }
        public bool NCAC512INSCH_ActiveFlg { get; set; }
        public long NCAC512INSCH_CreatedBy { get; set; }
        public long NCAC512INSCH_UpdatedBy { get; set; }
        public DateTime NCAC512INSCH_CreatedDate { get; set; }
        public DateTime NCAC512INSCH_UpdatedDate { get; set; }
        public string NCAC512INSCH_StatusFlg { get; set; }
        public List<NAAC_AC_512_InstScholarshipFilesDMO> NAAC_AC_512_InstScholarshipFilesDMO { get; set; }

    }
}
