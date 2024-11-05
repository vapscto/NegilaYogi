using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface StaffReplacementUnalocatedPeriodInterface
    {
        StaffReplacementUnalocatedPeriodDTO getdetails(int id);   
        StaffReplacementUnalocatedPeriodDTO get_catg(StaffReplacementUnalocatedPeriodDTO objcategory);
        Task<StaffReplacementUnalocatedPeriodDTO> getrpt(StaffReplacementUnalocatedPeriodDTO objcategory);
    }
}
