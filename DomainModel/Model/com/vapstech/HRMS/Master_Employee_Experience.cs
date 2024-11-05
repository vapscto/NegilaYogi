using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_Experience")]
    public class Master_Employee_Experience : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEE_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRMEE_OrganisationName { get; set; }
        public string HRMEE_OrganisationAddress { get; set; }
        public string HRMEE_Department { get; set; }
        public string HRMEE_Designation { get; set; }
        public DateTime? HRMEE_JoinDate { get; set; }
        public DateTime? HRMEE_ExitDate { get; set; }
        public int HRMEE_NoOfYears { get; set; }
        public int HRMEE_NoOfMonths { get; set; }
        public decimal? HRMEE_AnnualSalary { get; set; }
        public string HRMEE_ReasonForLeaving { get; set; }
    }
}
