using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_SMSNo", Schema = "CLG")]
    public class AdmCollegeStudentSMSNoDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTSMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACSTSMS_MobileNo { get; set; }
        public string ACSTSMS_CountryCode { get; set; }
    }
}
