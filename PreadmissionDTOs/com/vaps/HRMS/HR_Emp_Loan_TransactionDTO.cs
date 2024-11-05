using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_Loan_TransactionDTO
    {

        public long HRELT_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HREL_Id { get; set; }
        public int HRELT_Year { get; set; }
        public string HRELT_Month { get; set; }
        public bool HRELT_PaidFlag { get; set; }
        public decimal HRELT_LoanAmount { get; set; }
        public decimal HRELT_PrincipalAmount { get; set; }
        public decimal HRELT_InterestAmount { get; set; }
        public string HRELT_Reason { get; set; }

        public string retrunMsg { get; set; }

        public Array employeedropdown { get; set; }

        public Array leaveyeardropdown { get; set; }

        public Array monthdropdown { get; set; }
        public string hrmE_EmployeeFirstName { get; set; }
        public Array gridoption { get; set; }
                   
        public Array loandrop { get; set; }
        public long HRMLN_Id { get; set; }
        public bool returnval { get; set; }

    }
}
