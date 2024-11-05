using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_SubjectMarks", Schema = "CLG")]
    public class Adm_College_Student_SubjectMarksDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTSUM_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTSUM_SubjectName { get; set; }
        public decimal ACSTSUM_MaxMarks { get; set; }
        public decimal ACSTSUM_SubjectMarks { get; set; }
        public decimal ACSTSUM_Percentage { get; set; }
        public string ACSTSUM_LangFlg { get; set; }
    }
}
