using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
  public  interface ClgSMSEmailCountInterface
    {
        ClgSMSEmailCountDTO getdata(ClgSMSEmailCountDTO data);
        ClgSMSEmailCountDTO getreport(ClgSMSEmailCountDTO data);
        //SearchByColumn
        ClgSMSEmailCountDTO SearchByColumn(ClgSMSEmailCountDTO data);
    }
}
