using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_College_Student_SiblingsDetails", Schema = "CLG")]
    public class AdmCollegeStudentSiblingsDetailsDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACSTS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTS_SiblingsName { get; set; }
        public string ACSTS_SiblingsRelation { get; set; }
        public long AMCO_Id { get; set; }
        public long ACSTS_SiblingsAMCST_ID { get; set; }
        public int ACSTS_SiblingsOrder { get; set; }
        public bool ACSTS_TCIssuesFlag { get; set; }
    }
}
