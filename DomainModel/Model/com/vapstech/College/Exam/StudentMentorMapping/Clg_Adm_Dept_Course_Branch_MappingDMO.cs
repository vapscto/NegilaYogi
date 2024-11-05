using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Exam.StudentMentorMapping
{
    [Table("Adm_Dept_Course", Schema = "CLG")]
    public class Clg_Adm_Dept_Course_Branch_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ADCO_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long AMCO_Id { get; set; }       
        public bool ADCO_ActiveFlag { get; set; }
        public long ADCO_CreatedBy { get; set; }
        public long ADCO_UpdatedBy { get; set; }
        public List<Clg_Adm_Dept_Course_Branch_Semester_MappingDMO> Clg_Adm_Dept_Course_Branch_Semester_MappingDMO { get; set; }
    }
}
