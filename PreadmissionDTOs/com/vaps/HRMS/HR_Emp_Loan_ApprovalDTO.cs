using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_Loan_ApprovalDTO
    {

        public long HRELA_Id { get; set; }
        public long HREL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRESAA_Remarks { get; set; }
        public DateTime HRELA_Date { get; set; }
        public string HRELA_SanctionLevel { get; set; }
        public bool HRELA_ActiveFlag { get; set; }
        public long roleId { get; set; }
        public Array griddisplay { get; set; }

        public long MI_Id { get; set; }

        public string returnMsg { get; set; }
        public long LogInUserId { get; set; }

    }
}
