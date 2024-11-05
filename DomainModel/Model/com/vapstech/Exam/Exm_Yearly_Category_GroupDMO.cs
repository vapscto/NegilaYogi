using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Yearly_Category_Group", Schema = "Exm")]
    public class Exm_Yearly_Category_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EYCG_Id { get; set; }
        [ForeignKey("EYC_Id")]
        public int EYC_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool EYCG_ActiveFlg { get; set; }
       public List<Exm_Yearly_Category_Group_SubjectsDMO> Exm_Yearly_Category_Group_Subjects { get; set; }
    }
}
