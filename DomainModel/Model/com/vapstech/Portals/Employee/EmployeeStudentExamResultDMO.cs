using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Exm_M_Exam_Remarks", Schema = "Exm")]
    public class EmployeeStudentExamResultDMO:CommonParamDMO
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EMER_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int EME_Id { get; set; }
        public long AMST_Id { get; set; }
        public string EMER_Remarks { get; set; }
        public bool EMER_ActiveFlag { get; set; }
       


    }
}
