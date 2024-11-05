using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Exam
{
    [Table("Exm_Col_Yearly_Category_Group", Schema = "CLG")]
    public class Exm_Col_Yearly_Category_GroupDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ECYCG_Id { get; set; }
        public long ECYC_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool ECYCG_ActiveFlg { get; set; }
    }
}
