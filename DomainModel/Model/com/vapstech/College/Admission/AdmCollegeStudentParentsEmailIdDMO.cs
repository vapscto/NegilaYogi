using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Parents_EmailId",Schema ="CLG")]
    public class AdmCollegeStudentParentsEmailIdDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTPE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPE_EmailId { get; set; }
        public string ACSTPE_Flag { get; set; }
    }
}
