using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Exam.LessonPlanner
{
    [Table("LP_LessonPlanner_College", Schema = "CLG")]
    public class CollegeStaffPeriodMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPLPC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMTC_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }   
        public DateTime LPLPC_LPDate { get; set; }
        public DateTime LPLPC_CTDate { get; set; }
        public bool LPLPC_ClassTakenFlg { get; set; }
        public bool LPLPC_ActiveFlag { get; set; }
        public long LPMTC_CreatedBy { get; set; }
        public long LPMTC_UpdatedBy { get; set; }
    }
}
