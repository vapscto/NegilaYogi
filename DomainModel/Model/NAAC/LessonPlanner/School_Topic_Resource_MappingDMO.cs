using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam.LessonPlanner
{
    [Table("LP_Master_Topic_Resources")]
    public class School_Topic_Resource_MappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMTR_Id { get; set; }
        public long LPMT_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
        public string LPMTR_ResourceType { get; set; }
        public long LPMTR_CreatedBy { get; set; }
        public long LPMTR_UpdatedBy { get; set; }
    }
}
