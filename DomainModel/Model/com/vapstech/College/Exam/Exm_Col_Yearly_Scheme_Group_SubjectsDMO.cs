using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yearly_Scheme_Group_Subjects", Schema = "CLG")]
   public class Exm_Col_Yearly_Scheme_Group_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYSGS_Id { get; set; }
        public long ECYSG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ECYSGS_ActiveFlag { get; set; }
    }
}
