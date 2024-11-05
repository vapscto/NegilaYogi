using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_511_GovScholarship")]
    public class NAAC_AC_511_GovScholarshipDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC511GSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC511GSCH_Year { get; set; }
        public string NCAC511GSCH_SchemeName { get; set; }
        public long NCAC511GSCH_NoOfStudents { get; set; }
        public bool NCAC511GSCH_ActiveFlg { get; set; }
        public long NCAC511GSCH_CreatedBy { get; set; }
        public long NCAC511GSCH_UpdatedBy { get; set; }
        public DateTime NCAC511GSCH_CreatedDate { get; set; }
        public DateTime NCAC511GSCH_UpdatedDate { get; set; }
        public string NCAC511GSCH_StatusFlg { get; set; }

        public List<NAAC_AC_511_GovScholarshipFilesDMO> NAAC_AC_511_GovScholarshipFilesDMO { get; set; }

    }
}
