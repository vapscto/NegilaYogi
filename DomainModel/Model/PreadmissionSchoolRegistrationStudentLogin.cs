using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_StudentLogin")]
    public class PreadmissionSchoolRegistrationStudentLogin : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRSTUL_Id  { get; set; }
        public long IVRM_Others_User_Login_IVRMOUL_Id  { get; set; }
        public long Preadmission_School_Registration_PASR_Id  { get; set; }
        public long PASR_Id { get; set; }

        public long IVRMOUL_Id  { get; set; }
        public DateTime PASRSTUL_Date  { get; set; }
        public string  PASRSTUL_EntryType  { get; set; }
        public string PASRSTUL_MAACAdd { get; set; }

        public string PASRSTUL_IPAdd  { get; set; }
        public string PASRSTUL_NetIp  { get; set; }

    
    }
}
