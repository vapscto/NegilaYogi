using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Student_Guardian_Details")]
    public class IVRM_Student_Guardian_DetailsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISGD_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ISGD_GuardianName { get; set; }
        public string ISGD_GuardianAddress { get; set; }
        public long ISGD_GuardianPhoneNo { get; set; }
        public string ISGD_emailid { get; set; }
        public string ISGD_GuardianPhoto { get; set; }

    }
}
