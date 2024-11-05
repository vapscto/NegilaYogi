using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_MasterExam_GroupB")]
    public class HR_MasterExam_GroupBDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEGB_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMEGB_GroupBExamName { get; set; }
        public bool HRMEGB_ActiveFlg { get; set; }
        public long HRMEGB_CreatedBy { get; set; }
        public long HRMEGB_UpdatedBy { get; set; }
    }

}