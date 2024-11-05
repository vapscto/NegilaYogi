using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface StaffPeriodTransformInterface
    {
        StaffPeriodTransformDTO getdetails(int id);   
        StaffPeriodTransformDTO get_catg(StaffPeriodTransformDTO objcategory);
        StaffPeriodTransformDTO getreport(StaffPeriodTransformDTO objcategory);
        StaffPeriodTransformDTO gettimetable(StaffPeriodTransformDTO objcategory);
        StaffPeriodTransformDTO getpossiblePeriod(StaffPeriodTransformDTO objcategory);
        StaffPeriodTransformDTO savedetail(StaffPeriodTransformDTO objperiod);
        StaffPeriodTransformDTO deleteperiod(StaffPeriodTransformDTO objperiod);
    }
}
