using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_Master_Student_Guardian")]
    public class StudentGuardianDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSTG_Id { get; set; }
        public long MI_Id { get; set; }
        [ForeignKey("AMST_Id")]
        public long AMST_Id { get; set; }
        public string AMSTG_GuardianName { get; set; }
        public string AMSTG_GuardianAddress { get; set; }
        public string AMSTG_GuardianPhoneNo { get; set; }
        public string AMSTG_emailid { get; set; }
        public string AMSTG_GuardianPhoto { get; set; }
        public string AMSTG_GuardianSign { get; set; }
        public string AMSTG_Fingerprint { get; set; }
        public string AMSTG_GuardianLoginFlag { get; set; }
        public long? AMSTG_CreatedBy { get; set; }
        public long? AMSTG_UpdatedBy { get; set; }
    }
}
