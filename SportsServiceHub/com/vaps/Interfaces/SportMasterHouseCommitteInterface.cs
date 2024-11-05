using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportMasterHouseCommitteInterface
    {
        HouseCommitte_DTO mastercasteData(HouseCommitte_DTO mas);

        HouseCommitte_DTO get_section(HouseCommitte_DTO mas);
        HouseCommitte_DTO get_student(HouseCommitte_DTO mas);

        HouseCommitte_DTO deactivate(HouseCommitte_DTO dto);

        HouseCommitte_DTO GetSelectedRowDetails(HouseCommitte_DTO dto);

        HouseCommitte_DTO GetmastercasteData(HouseCommitte_DTO HouseCommitte_DTO);

        HouseCommitte_DTO onhousechage(HouseCommitte_DTO dto);
        HouseCommitte_DTO get_House(HouseCommitte_DTO dto);
        
    }
}
