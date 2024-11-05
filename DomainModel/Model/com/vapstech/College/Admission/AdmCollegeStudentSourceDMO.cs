using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Source", Schema = "CLG")]
    public class AdmCollegeStudentSourceDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASRS_Id { get; set; }
    }
}
