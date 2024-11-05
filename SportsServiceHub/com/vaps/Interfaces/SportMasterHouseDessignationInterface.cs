using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportMasterHouseDessignationInterface
    {
        SPCC_Master_House_Designation_DTO mastercasteData(SPCC_Master_House_Designation_DTO mas);

        SPCC_Master_House_Designation_DTO deactivate(SPCC_Master_House_Designation_DTO dto);

        SPCC_Master_House_Designation_DTO GetSelectedRowDetails(int ID);

        SPCC_Master_House_Designation_DTO GetmastercasteData(SPCC_Master_House_Designation_DTO SPCC_Master_House_Designation_DTO);
    }
}
