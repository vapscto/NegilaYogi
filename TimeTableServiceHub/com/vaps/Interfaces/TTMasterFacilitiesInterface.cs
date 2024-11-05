using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface TTMasterFacilitiesInterface
    {
        TTMasterFacilitiesDTO savedetail(TTMasterFacilitiesDTO objcategory);
        TTMasterFacilitiesDTO getdetails(int id);
        TTMasterFacilitiesDTO getpageedit(int id);
        TTMasterFacilitiesDTO deactivate(TTMasterFacilitiesDTO id);
    }
}
