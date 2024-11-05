using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.LessonPlanner
{
    [Table("LP_Master_MainTopic_College")]
    public class LP_Master_MainTopic_CollegeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPMMTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long LPMU_Id { get; set; }
        public string LPMMTC_TopicName { get; set; }
        public string LPMMTC_TopicDescription { get; set; }
        public long LPMMTC_TotalHrs { get; set; }
        public long LPMMTC_TotalPeriods { get; set; }
        public bool LPMMTC_ActiveFlg { get; set; }
        public long LPMMTC_CreatedBy { get; set; }
        public long LPMMTC_UpdatedBy { get; set; }
        public int LPMMTC_Order { get; set; }
    }
}
