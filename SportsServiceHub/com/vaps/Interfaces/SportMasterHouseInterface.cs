using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportMasterHouseInterface
    {
        SportMasterHouseDTO mastercasteData(SportMasterHouseDTO mas);
        
        SportMasterHouseDTO deactivate(SportMasterHouseDTO dto);

        SportMasterHouseDTO GetSelectedRowDetails(int ID);

        SportMasterHouseDTO GetmastercasteData(SportMasterHouseDTO SportMasterHouseDTO);
    }
}
