using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_EarningsDeductions_TypeDTO:CommonParamDTO
    {
    public long HRMEDT_Id { get; set; }
    public string HRMEDT_EarnDedType { get; set; }
    public bool HRMEDT_ActiveFlag { get; set; }
    public Array eardettypelist { get; set; }
    public string retrunMsg { get; set; }

    public long roleId { get; set; }
  }
}
