using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface StaffReplacementForClassSectionInterface
    {
        TTStaffReplacementForClassSectionDTO getdetails(int id);
        TTStaffReplacementForClassSectionDTO get_catg(TTStaffReplacementForClassSectionDTO objcategory);
        TTStaffReplacementForClassSectionDTO getclass_catg(TTStaffReplacementForClassSectionDTO objcategory);
        TTStaffReplacementForClassSectionDTO getreport(TTStaffReplacementForClassSectionDTO objcategory);
        TTStaffReplacementForClassSectionDTO getpossiblePeriod(TTStaffReplacementForClassSectionDTO objcategory);
        TTStaffReplacementForClassSectionDTO savedetail(TTStaffReplacementForClassSectionDTO objperiod);
    }
}
