using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Reference", Schema = "CLG")]
    public class AdmCollegeStudentReferenceDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTR_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASRR_Id { get; set; }
    }
}
