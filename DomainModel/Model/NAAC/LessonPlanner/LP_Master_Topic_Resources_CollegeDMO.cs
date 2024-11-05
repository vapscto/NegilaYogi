using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.LessonPlanner
{
    [Table("LP_Master_Topic_Resources_College")]
    public class LP_Master_Topic_Resources_CollegeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMTRC_Id { get; set; }
        public long LPMTC_Id { get; set; }
        public string LPMTRC_Resources { get; set; }
        public string LPMTRC_ResourceType { get; set; }
        public string LPMTRC_FileName { get; set; }
        public long LPMTRC_CreatedBy { get; set; }
        public long LPMTRC_UpdatedBy { get; set; }
    }
}
