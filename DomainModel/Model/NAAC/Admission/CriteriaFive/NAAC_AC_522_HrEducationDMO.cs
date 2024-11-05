using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_522_HrEducation")]
    public class NAAC_AC_522_HrEducationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long NCAC522HRED_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC522HRED_Year { get; set; }
        public string NCAC522HRED_HrEduEnrollStudentNo { get; set; }
        public long NCAC522HRED_GraduatedProgram { get; set; }
        public long NCAC522HRED_GraduatedDept { get; set; }
        public string NCAC522HRED_InstitutionName { get; set; }
        public string NCAC522HRED_AdmittedProgram { get; set; }
        public string NCAC522HRED_AdmittedDept { get; set; }
        public string NCAC522HRED_StatusFlg { get; set; }
        public bool NCAC522HRED_ActiveFlg { get; set; }
        public long NCAC522HRED_CreatedBy { get; set; }
        public long NCAC522HRED_UpdatedBy { get; set; }
        public DateTime NCAC522HRED_CreatedDate { get; set; }
        public DateTime NCAC522HRED_UpdatedDate { get; set; }

        public List<NAAC_AC_522_HrEducationFilesDMO> NAAC_AC_522_HrEducationFilesDMO { get; set; }

    }
}
