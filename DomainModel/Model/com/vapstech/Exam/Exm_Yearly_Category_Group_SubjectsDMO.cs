using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yearly_Category_Group_Subjects", Schema = "Exm")]
    public class Exm_Yearly_Category_Group_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     

        public int EYCGS_Id { get; set; }
        [ForeignKey("EYCG_Id")]
        public int EYCG_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool EYCGS_ActiveFlg { get; set; }
       
    }
}
