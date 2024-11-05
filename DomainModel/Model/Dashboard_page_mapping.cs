using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("ivrm_dashboard_page_mapping")]
    public class Dashboard_page_mapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_DBID { get; set; }
        public string IVRMRT_Role { get; set; }
        public long MI_ID { get; set; }
        public string IVRMP_Dasboard_PageName { get; set; }

        public DateTime? IVRM_CreatedDate { get; set; }
        public DateTime? IVRM_UpdatedDate { get; set; }
        public long? IVRM_CreatedBy { get; set; }
        public long? IVRM_UpdatedBy { get; set; }
    }
}
