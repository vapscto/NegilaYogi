using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Emp_SalaryAdvance_ApprovalDTO
    {


        public long HRESAA_Id { get; set; }
        public long HRESA_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRESAA_Remarks { get; set; }
        public DateTime HRESAA_Date { get; set; }
        public string HRESAA_SanctionLevel { get; set; }
        public bool HRESAA_ActiveFlag { get; set; }
     
    }
}
