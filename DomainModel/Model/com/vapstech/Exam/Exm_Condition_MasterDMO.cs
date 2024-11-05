using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Condition_Master", Schema = "Exm")]
    public class Exm_Condition_MasterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ECM_Id { get; set; }
        public long MI_Id { get; set; }  
        public string ECM_ConditionName { get; set; }      
        public string ECM_ConditionFlag { get; set; }
        public bool ECM_ActiveFlg { get; set; }
        
    }
}
