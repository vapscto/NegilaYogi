using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
  public  interface StaffReplacementInTheirTTInterface
    {
        TTStaffReplacementInTheirTTDTO getdetails(int id);   
        TTStaffReplacementInTheirTTDTO get_catg(TTStaffReplacementInTheirTTDTO objcategory);
        TTStaffReplacementInTheirTTDTO getreport(TTStaffReplacementInTheirTTDTO objcategory);
        TTStaffReplacementInTheirTTDTO getpossiblePeriod(TTStaffReplacementInTheirTTDTO objcategory);
        TTStaffReplacementInTheirTTDTO savedetail(TTStaffReplacementInTheirTTDTO objperiod);
    }
}
