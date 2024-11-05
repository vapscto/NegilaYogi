using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{


  public  interface ClgPeriodAllocationInterface
    {
      //  ClgPeriodAllocation_DTO savedetail(ClgPeriodAllocation_DTO objperiod);
        ClgPeriodAllocation_DTO save_period(ClgPeriodAllocation_DTO period);
        //ClgPeriodAllocation_DTO getcategories(ClgPeriodAllocation_DTO period);
        //ClgPeriodAllocation_DTO getBranch(ClgPeriodAllocation_DTO period);
        ClgPeriodAllocation_DTO getdetails(ClgPeriodAllocation_DTO data);
        ClgPeriodAllocation_DTO getcategories(ClgPeriodAllocation_DTO data);
        ClgPeriodAllocation_DTO getperiod_class(ClgPeriodAllocation_DTO data);
        ClgPeriodAllocation_DTO savedetail(ClgPeriodAllocation_DTO data);
        ClgPeriodAllocation_DTO deactivate(ClgPeriodAllocation_DTO data);
        ClgPeriodAllocation_DTO deactivate1(ClgPeriodAllocation_DTO data);

    }
}
