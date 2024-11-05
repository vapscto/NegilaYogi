using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("LP_Master_MainTopic")]
    public class MasterSchoolTopicDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMU_Id { get; set; }
        public long ISMS_Id { get; set; }
        public string LPMMT_TopicName { get; set; }
        public string LPMMT_TopicDescription { get; set; }
        public long? LPMMT_TotalHrs { get; set; }
        public long? LPMMT_TotalPeriods { get; set; }
        public int LPMMT_Order { get; set; }
        public bool LPMMT_ActiveFlag { get; set; }
        public long LPMMT_CreatedBy { get; set; }
        public long LPMMT_UpdatedBy { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
    }
}
