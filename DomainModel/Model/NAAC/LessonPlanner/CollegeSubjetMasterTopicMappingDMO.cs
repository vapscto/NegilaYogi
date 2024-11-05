using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.College.Exam.LessonPlanner
{
    [Table("LP_Master_Topic")]
    public class CollegeSubjetMasterTopicMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }
        public string LPMT_LessonPlan { get; set; }
        public long LPMT_TotalHrs { get; set; }
        public long LPMT_TotalPeriods { get; set; }
        public string LPMT_TeacherGuide { get; set; }
        public string LPMT_StudentGuide { get; set; }
        public string LPMT_MaterialNeeded { get; set; }
        public string LPMT_References { get; set; }
        public string LPMT_Homework { get; set; }
        public int LPMT_TopicOrder { get; set; }
        public bool LPMT_ActiveFlag { get; set; }
        public long LPMT_CreatedBy { get; set; }
        public long LPMT_UpdatedBy { get; set; }
    }
}
