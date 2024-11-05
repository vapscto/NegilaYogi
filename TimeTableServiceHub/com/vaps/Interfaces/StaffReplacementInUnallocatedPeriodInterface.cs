using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface StaffReplacementInUnallocatedPeriodInterface
    {
        TTStaffReplacementInUnallocatedPeriodDTO getdetails(int id);   
        TTStaffReplacementInUnallocatedPeriodDTO get_catg(TTStaffReplacementInUnallocatedPeriodDTO objcategory);
        TTStaffReplacementInUnallocatedPeriodDTO getreport(TTStaffReplacementInUnallocatedPeriodDTO objcategory);
        TTStaffReplacementInUnallocatedPeriodDTO savedetail(TTStaffReplacementInUnallocatedPeriodDTO objperiod);
    }
}
