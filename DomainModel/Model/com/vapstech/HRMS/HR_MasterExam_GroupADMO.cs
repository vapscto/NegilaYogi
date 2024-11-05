using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_MasterExam_GroupA")]
    public class HR_MasterExam_GroupADMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEGA_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMEGA_GroupAExamName { get; set; }
        public bool HRMEGA_ActiveFlg { get; set; }
        public long HRMEGA_CreatedBy { get; set; }
        public long HRMEGA_UpdatedBy { get; set; }
    }

}