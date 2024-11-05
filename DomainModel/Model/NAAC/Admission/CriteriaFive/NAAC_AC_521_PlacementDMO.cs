using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_521_Placement")]
    public class NAAC_AC_521_PlacementDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  NCAC521PLA_Id { get; set; }
        public long  MI_Id { get; set; }
        public long  NCAC521PLA_Year { get; set; }
        public long  NCAC521PLA_NoOfStudents { get; set; }
        public long NCAC521PLA_NoOfstudentsselfemployed { get; set; }
        public string  NCAC521PLA_EmployerName { get; set; }
        public string  NCAC521PLA_Package { get; set; }
        public string NCAC521PLA_StatusFlg { get; set; }
        public long  NCAC521PLA_GradCourse { get; set; }
        public long  NCAC521PLA_GradBranch { get; set; }
        public bool  NCAC521PLA_ActiveFlg { get; set; }
        public long  NCAC521PLA_CreatedBy { get; set; }
        public long  NCAC521PLA_UpdatedBy { get; set; }
        public DateTime  NCAC521PLA_CreatedDate { get; set; }
        public DateTime NCAC521PLA_UpdatedDate { get; set; }
        public List<NAAC_AC_521_PlacementFilesDMO> NAAC_AC_521_PlacementFilesDMO { get; set; }

    }
}
