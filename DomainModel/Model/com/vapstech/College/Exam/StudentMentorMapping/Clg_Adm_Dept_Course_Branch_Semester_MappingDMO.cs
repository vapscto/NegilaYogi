using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam.StudentMentorMapping
{
    [Table("Adm_Dept_Course_Branch_Semester", Schema = "CLG")]
    public class Clg_Adm_Dept_Course_Branch_Semester_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ADCOBS_Id { get; set; }
        public long ADCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool ADCOBS_ActiveFlag { get; set; }
        public long ADCOBS_CreatedBy { get; set; }
        public long ADCOBS_UpdatedBy { get; set; }
    }
}
