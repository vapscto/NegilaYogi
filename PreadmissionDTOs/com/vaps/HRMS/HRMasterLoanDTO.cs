using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HRMasterLoanDTO:CommonParamDTO
    {
    public long HRMLN_Id { get; set; }
    public long MI_Id { get; set; }
    public string HRML_LoanType { get; set; }
        public decimal? HRML_Max { get; set; }
        public bool HRMLN_ActiveFlag { get; set; }
    public Array gmasterloanList { get; set; }
    public string retrunMsg { get; set; }
    public long roleId { get; set; }
  }
}
