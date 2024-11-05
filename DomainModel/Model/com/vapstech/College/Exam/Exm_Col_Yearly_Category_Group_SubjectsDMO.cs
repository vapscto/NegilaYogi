using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yearly_Category_Group_Subjects", Schema = "CLG")]
    public class Exm_Col_Yearly_Category_Group_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYCGS_Id { get; set; }
        public long ECYCG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECYCGS_ActiveFlg { get; set; }
    }
}
