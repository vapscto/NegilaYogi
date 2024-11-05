using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Exam.LessonPlanner
{
    [Table("LP_Master_Unit")]
    public class SchoolMasterUnitDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMU_Id { get; set; }
        public long MI_Id { get; set; }
        public string LPMU_UnitName { get; set; }
        public string LPMU_UnitDescription { get; set; }
        public decimal? LPMU_TotalHrs { get; set; }
        public long? LPMU_TotalPeriods { get; set; }
        public int LPMU_Order { get; set; }
        public bool LPMU_ActiveFlag { get; set; }
        public long LPMU_CreatedBy { get; set; }
        public long LPMU_UpdatedBy { get; set; }
    }
}
