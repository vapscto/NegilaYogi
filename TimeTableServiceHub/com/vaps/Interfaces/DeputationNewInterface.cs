using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface DeputationNewInterface
    {
        TTDeputationDTO savedetails(TTDeputationDTO objcategory);
        TTDeputationDTO viewabsent(TTDeputationDTO objcategory);
        TTDeputationDTO getabsentstaff(TTDeputationDTO objcategory);
        TTDeputationDTO get_period_alloted(TTDeputationDTO objcategory);
        TTDeputationDTO get_free_stfdets(TTDeputationDTO objcategory);
        TTDeputationDTO getalldetailsviewrecords2(TTDeputationDTO objcategory);
        TTDeputationDTO viewrecordspopup9(TTDeputationDTO objcategory);
        TTDeputationDTO viewdeputation(TTDeputationDTO objcategory);
        TTDeputationDTO getdetails(int id);
    }
}
