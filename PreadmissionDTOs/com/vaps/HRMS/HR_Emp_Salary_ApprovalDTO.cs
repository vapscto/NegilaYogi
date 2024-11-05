using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.HRMS
{
  public  class HR_Emp_Salary_ApprovalDTO : CommonParamDTO
    {
        public long HRESA_Id { get; set; }
        public long HRES_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string HRESA_Remarks { get; set; }
        public long HRESA_SanctionLevel { get; set; }
        public bool HRESA_ActiveFlag { get; set; }
        public DateTime? HRESA_Date { get; set; }
    }
}
