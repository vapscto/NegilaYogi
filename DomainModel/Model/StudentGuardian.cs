using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_Guardian")]
    public class StudentGuardian : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRG_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASRG_GuardianName { get; set; }
        public string PASRG_GuardianAddress { get; set; }
        public long? PASRG_GuardianPhoneNo { get; set; }
        public string PASRG_emailid { get; set; }
        public int PASRG_GuardianLoginFlag { get; set; }
        public string PASRG_GuardianRelation  { get; set; }
        public string PASRG_Occupation  { get; set; }
        public long? PASRG_PhoneOffice  { get; set; }
        public bool PASRG_FeeUndertakeFlg { get; set; }
    }
}
