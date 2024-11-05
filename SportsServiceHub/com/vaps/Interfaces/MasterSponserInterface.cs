using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface MasterSponserInterface
    {
        MasterSponserDTO getDetails(MasterSponserDTO data);
        MasterSponserDTO saveRecord(MasterSponserDTO obj);
        MasterSponserDTO EditDetails(int id);
        MasterSponserDTO deactivateSponser(MasterSponserDTO obj);
        


    }
}
