using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.LessonPlanner
{
    [Table("LP_Master_Topic_College")]
    public class LP_Master_Topic_CollegeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMMTC_Id { get; set; }
        public string LPMTC_TopicName { get; set; }
        public string LPMTC_LessonPlan { get; set; }
        public long LPMTC_TotalHrs { get; set; }
        public long LPMTC_TotalPeriods { get; set; }
        public string LPMTC_TeacherGuide { get; set; }
        public string LPMTC_StudentGuide { get; set; }
        public string LPMTC_MaterialNeeded { get; set; }
        public string LPMTC_References { get; set; }
        public string LPMTC_Homework { get; set; }
        public int LPMTC_TopicOrder { get; set; }
        public long LPMTC_CreatedBy { get; set; }
        public long LPMTC_UpdatedBy { get; set; }
        public bool LPMTC_Activefalg { get; set; }
        public List<LP_Master_Topic_Resources_CollegeDMO> LP_Master_Topic_Resources_CollegeDMO { get; set; }

    }
}
