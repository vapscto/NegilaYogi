using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Login_Privilege", Schema = "Exm")]
    public class Exm_Login_PrivilegeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ELP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int Login_Id { get; set; }
        public string ELP_Flg { get; set; }
        public bool ELP_ActiveFlg { get; set; }

        public List<Exm_Login_Privilege_SubjectsDMO> Exm_Login_Privilege_Subjects { get; set; }

    }
}
