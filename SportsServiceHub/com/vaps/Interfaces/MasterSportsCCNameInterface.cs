using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
  public  interface MasterSportsCCNameInterface
    {
        MasterSportsCCNameDTO getDetails(MasterSportsCCNameDTO data);
        MasterSportsCCNameDTO saveRecord(MasterSportsCCNameDTO obj);
        MasterSportsCCNameDTO EditDetails(int id);
        MasterSportsCCNameDTO deactivate(MasterSportsCCNameDTO obj);
    }
}
