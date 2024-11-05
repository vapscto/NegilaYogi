using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface CLGStaffPeriodTransformInterface
    {
        CLGStaffPeriodTransformDTO getdetails(CLGStaffPeriodTransformDTO objcategory);   
        CLGStaffPeriodTransformDTO get_catg(CLGStaffPeriodTransformDTO objcategory);
        CLGStaffPeriodTransformDTO getreport(CLGStaffPeriodTransformDTO objcategory);
        CLGStaffPeriodTransformDTO gettimetable(CLGStaffPeriodTransformDTO objcategory);
        CLGStaffPeriodTransformDTO getpossiblePeriod(CLGStaffPeriodTransformDTO objcategory);
        CLGStaffPeriodTransformDTO savedetail(CLGStaffPeriodTransformDTO objperiod);
        CLGStaffPeriodTransformDTO deleteperiod(CLGStaffPeriodTransformDTO objperiod);
    }
}
