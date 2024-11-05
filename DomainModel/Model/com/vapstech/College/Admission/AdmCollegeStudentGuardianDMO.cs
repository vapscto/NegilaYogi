using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_Guardian", Schema = "CLG")]
    public class AdmCollegeStudentGuardianDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTG_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTG_GuardianName { get; set; }
        public string ACSTG_GuardianAddress { get; set; }
        public long ACSTG_GuardianPhoneNo { get; set; }
        public string ACSTG_emailid { get; set; }
        public string ACSTG_GuardianPhoto { get; set; }
        public string ACSTG_GuardianSign { get; set; }
        public string ACSTG_Fingerprint { get; set; }
       
        public string ACSTG_CoutryCode { get; set; }
        public bool? ACSTG_GuardianLoginFlag { get; set; }
    }
}
