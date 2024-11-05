using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface SportsMasterHouseInterface
    {

        SportsMasterHouse_DTO getdetails(SportsMasterHouse_DTO dTO);      
        SportsMasterHouse_DTO savedata(SportsMasterHouse_DTO mas);
        SportsMasterHouse_DTO deactivate(SportsMasterHouse_DTO mas);
        SportsMasterHouse_DTO editdata(SportsMasterHouse_DTO mas);


    }
}
