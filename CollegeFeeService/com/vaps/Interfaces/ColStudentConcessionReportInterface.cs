using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface ColStudentConcessionReportInterface
    {
        CollegeConcessionDTO getalldetails(CollegeConcessionDTO data);
    }
}
