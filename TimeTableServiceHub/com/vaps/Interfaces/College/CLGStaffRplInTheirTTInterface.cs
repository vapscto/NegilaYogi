using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface CLGStaffRplInTheirTTInterface
    {
        CLGStaffRplInTheirTTDTO getdetails(CLGStaffRplInTheirTTDTO objcategory);   
        CLGStaffRplInTheirTTDTO get_catg(CLGStaffRplInTheirTTDTO objcategory);
        CLGStaffRplInTheirTTDTO getreport(CLGStaffRplInTheirTTDTO objcategory);
        CLGStaffRplInTheirTTDTO getpossiblePeriod(CLGStaffRplInTheirTTDTO objcategory);
        CLGStaffRplInTheirTTDTO savedetail(CLGStaffRplInTheirTTDTO objperiod);
    }
}
