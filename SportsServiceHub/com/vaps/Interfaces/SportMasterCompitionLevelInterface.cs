using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportMasterCompitionLevelInterface
    {
        SportMasterCompitionLevelDTO mastercasteData(SportMasterCompitionLevelDTO mas);

        //SportMasterCompitionLevelDTO MasterDeleteModulesData(int ID);
        SportMasterCompitionLevelDTO deactivate(SportMasterCompitionLevelDTO dto);

        SportMasterCompitionLevelDTO GetSelectedRowDetails(int ID);

        SportMasterCompitionLevelDTO GetmastercasteData(SportMasterCompitionLevelDTO SportMasterCompitionLevelDTO);
    }
}
