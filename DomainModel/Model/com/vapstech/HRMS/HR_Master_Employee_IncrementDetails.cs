using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_IncrementDetails")]
    public class HR_Master_Employee_IncrementDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEID_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? HRMEID_Date { get; set; }
        public decimal? HRMEID_Amount { get; set; }
        public string HRMEID_PayScaleRange { get; set; }
        public decimal? HRMEID_IncrementAmount { get; set; }
        public int HRMEID_AfterYear { get; set; }
        public int HRMEID_AfterMonth { get; set; }
        public int HRMEID_AfterDays { get; set; }
    }
}
