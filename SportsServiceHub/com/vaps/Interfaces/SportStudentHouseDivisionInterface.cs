using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface SportStudentHouseDivisionInterface
    {
        SportMasterHouseDTO mastercasteData(SportMasterHouseDTO mas);

        SportMasterHouseDTO get_section(SportMasterHouseDTO mas);
        SportMasterHouseDTO get_student(SportMasterHouseDTO mas);

        SportMasterHouseDTO deactivate(SportMasterHouseDTO dto);

        SportMasterHouseDTO GetSelectedRowDetails(SportMasterHouseDTO dto);

        SportMasterHouseDTO GetmastercasteData(SportMasterHouseDTO SportMasterHouseDTO);
    }
}
