using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface MasterSportsCCGroupInterface
    {
        MasterSportsCCGroupDTO getDetails(MasterSportsCCGroupDTO data);
        MasterSportsCCGroupDTO saveRecord(MasterSportsCCGroupDTO obj);
        MasterSportsCCGroupDTO EditDetails(int id);
        MasterSportsCCGroupDTO deactivate(MasterSportsCCGroupDTO obj);
    }
}
