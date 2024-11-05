using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Login_Privilege_Subjects", Schema = "Exm")]
    public class Exm_Login_Privilege_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ELPS_Id { get; set; }
        [ForeignKey("ELP_Id")]
        public int ELP_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ELPs_ActiveFlg { get; set; }
        public Exm_Login_PrivilegeDMO ExamLoginPre { get; set; }
        public List<Exm_Login_Privilege_SubSubjectsDMO> Exm_Login_Privilege_SubSubjects { get; set; }
    }
}
