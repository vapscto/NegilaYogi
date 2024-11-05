using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Parents_MobileNo", Schema = "CLG")]
    public class AdmCollegeStudentParentsMobileNoDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTPMN_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACSTPMN_MobileNo { get; set; }
        public string ACSTPMN_Flag { get; set; }
        public string ACSTPMN_CountryCode { get; set; }
    }
}
