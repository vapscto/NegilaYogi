using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{

    public class HR_Emp_SalaryAdvanceDTO : CommonParamDTO
    {
        public long HRESA_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HRESA_EntryDate { get; set; }
        public int HRESA_AdvYear { get; set; }
        public string HRESA_AdvMonth { get; set; }
        public decimal? HRESA_AppliedAmount { get; set; }
        public string HRESA_Remarks { get; set; }
        public bool HRESA_ActiveFlag { get; set; }
        public string HRESA_AdvStatus { get; set; }

        public decimal? HRESA_SanctinedAmount { get; set; }
        public string HRESA_ModeOfPayment { get; set; }
        public long HRESA_ReferenceNo { get; set; }

        public Array empadvaList { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public Array griddisplay { get; set; }


        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }


        public Array employeedropdown { get; set; }

        public Array leaveyeardropdown { get; set; }
        public Array monthdropdown { get; set; }
        public Array modeOfPaymentdropdown { get; set; }

        public HR_ConfigurationDTO configurationDetails { get; set; }

        public decimal? empGrossSal { get; set; }
        public decimal? totalAppliedAmount { get; set; }
        public string searchfilter { get; set; }

    }
}

