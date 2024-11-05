using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Exam.LessonPlanner
{
    [Table("LP_Master_Topic_Unit")]
    public class SchoolMasterTopicUnitDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMTU_Id { get; set; }
        public long LPMU_Id { get; set; }
        public long LPMT_Id { get; set; }
        public bool LPMUT_ActiveFlag { get; set; }
        public long LPMTU_CreatedBy { get; set; }
        public long LPMTU_UpdatedBy { get; set; }
    }
}
