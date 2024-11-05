using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_LeaveYear")]
    public class HR_Master_LeaveYearDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMLY_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public DateTime? HRMLY_FromDate { get; set; }
        public DateTime? HRMLY_ToDate { get; set; }
        public bool? HRMLY_ActiveFlag { get; set; }
        public long HRMLY_LeaveYearOrder { get; set; }
    }
}
