
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Login_Privilege_SubSubjects", Schema = "Exm")]
    public class Exm_Login_Privilege_SubSubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ELPSS_Id { get; set; }
             [ForeignKey("ELPS_Id")]
        public int ELPS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public bool ELPSS_ActiveFlg { get; set; }

        public Exm_Login_Privilege_SubjectsDMO ExamLoginPresub { get; set; }



    }
}
