using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class Master_Employee_ExperienceDTO :CommonParamDTO
    {
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


        public string retrunMsg { get; set; }

    }
}
