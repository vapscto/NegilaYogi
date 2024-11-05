using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_User_Login_Student_College")]
    public  class CollegeStudentlogin :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMULSPGC_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long AMCST_Id  { get; set; }
        public bool IVRMULSPGC_ActiveFlag { get; set; }
        public string IVRMULSPGC_Flag { get; set; }

    }
}
