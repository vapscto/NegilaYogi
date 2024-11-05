using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface CLGStaffRplInUnallocatedPeriodInterface
    {
        CLGStaffRplInUnallocatedPeriodDTO getdetails(CLGStaffRplInUnallocatedPeriodDTO data);   
        CLGStaffRplInUnallocatedPeriodDTO get_catg(CLGStaffRplInUnallocatedPeriodDTO objcategory);
        CLGStaffRplInUnallocatedPeriodDTO getreport(CLGStaffRplInUnallocatedPeriodDTO objcategory);
        CLGStaffRplInUnallocatedPeriodDTO savedetail(CLGStaffRplInUnallocatedPeriodDTO objperiod);
    }
}
