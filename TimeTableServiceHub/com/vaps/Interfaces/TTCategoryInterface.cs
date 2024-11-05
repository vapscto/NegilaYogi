using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public  interface TTCategoryInterface
    {

        TTMasterCategoryDTO savedetail(TTMasterCategoryDTO objcategory);
        TTMasterCategoryDTO getdetails(int id);
        TTMasterCategoryDTO getpageedit(int id);
        TTMasterCategoryDTO deleterec(int id);
        TTMasterCategoryDTO deactivate(TTMasterCategoryDTO id);
    }
}
