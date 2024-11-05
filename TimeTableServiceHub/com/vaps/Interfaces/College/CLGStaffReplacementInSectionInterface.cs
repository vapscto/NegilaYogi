using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface CLGStaffReplacementInSectionInterface
    {
        CLGStaffReplacementInSectionDTO getdetails(CLGStaffReplacementInSectionDTO objcategory);
        CLGStaffReplacementInSectionDTO get_catg(CLGStaffReplacementInSectionDTO objcategory);
        CLGStaffReplacementInSectionDTO getclass_catg(CLGStaffReplacementInSectionDTO objcategory);
        CLGStaffReplacementInSectionDTO getreport(CLGStaffReplacementInSectionDTO objcategory);
        CLGStaffReplacementInSectionDTO getpossiblePeriod(CLGStaffReplacementInSectionDTO objcategory);
        CLGStaffReplacementInSectionDTO savedetail(CLGStaffReplacementInSectionDTO objperiod);
    }
}
