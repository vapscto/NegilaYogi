using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface TTGenerationFromPreviousVersionInterface
    {
        TTTTGenerationFromPreviousVersionDTO savedetail_(TTTTGenerationFromPreviousVersionDTO objcategory);
        TTTTGenerationFromPreviousVersionDTO deactivate(TTTTGenerationFromPreviousVersionDTO data);
        TTTTGenerationFromPreviousVersionDTO getdetails(int id);
        TTTTGenerationFromPreviousVersionDTO getpageedit(int id);
        TTTTGenerationFromPreviousVersionDTO deleterec(int id);


    }
}
