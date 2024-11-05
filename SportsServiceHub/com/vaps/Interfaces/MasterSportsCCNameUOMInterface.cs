using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public  interface MasterSportsCCNameUOMInterface
    {
        MasterSportsCCNameUMO_DTO getDetails(MasterSportsCCNameUMO_DTO data);
        MasterSportsCCNameUMO_DTO saveRecord(MasterSportsCCNameUMO_DTO obj);
        MasterSportsCCNameUMO_DTO EditDetails(int id);
        MasterSportsCCNameUMO_DTO deactivate(MasterSportsCCNameUMO_DTO obj);
    }
}
