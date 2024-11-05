using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_Group", Schema = "Exm")]
    public class Exm_Master_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public int EMG_TotSubjects { get; set; }
        public int EMG_MaxAplSubjects { get; set; }
        public int EMG_MinAplSubjects { get; set; }
        public int EMG_BestOff { get; set; }
    //    public int EMGR_Id { get; set; }
        public bool EMG_ActiveFlag { get;set;}
        public bool EMG_ElectiveFlg { get; set; }
        public List<Exm_Master_Group_SubjectsDMO> Exm_M_Group_Subjects { get; set; }
    }
}
