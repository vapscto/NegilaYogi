using PreadmissionDTOs.com.vaps.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportServiceHub.com.vaps.Interfaces
{
    public interface SportMasterDivisionInterface
    {
        SportMasterDivisionDTO mastercasteData(SportMasterDivisionDTO mas);

        //SportMasterDivisionDTO MasterDeleteModulesData(int ID);
        SportMasterDivisionDTO deactivate(SportMasterDivisionDTO dto);

        SportMasterDivisionDTO GetSelectedRowDetails(int ID);

        SportMasterDivisionDTO GetmastercasteData(SportMasterDivisionDTO SportMasterDivisionDTO);
    }
}
