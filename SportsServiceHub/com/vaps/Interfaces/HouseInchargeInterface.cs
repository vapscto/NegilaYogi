using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
   public interface HouseInchargeInterface
    {
        SPCC_Master_House_Staff_DTO Getdetails(SPCC_Master_House_Staff_DTO data);
        SPCC_Master_House_Staff_DTO saverecord(SPCC_Master_House_Staff_DTO data);
        SPCC_Master_House_Staff_DTO editrecord(SPCC_Master_House_Staff_DTO id);
        SPCC_Master_House_Staff_DTO deactive(SPCC_Master_House_Staff_DTO data);
        SPCC_Master_House_Staff_DTO get_House(SPCC_Master_House_Staff_DTO data);
        SPCC_Master_House_Staff_DTO getdepchange(SPCC_Master_House_Staff_DTO data);
        SPCC_Master_House_Staff_DTO get_staff1(SPCC_Master_House_Staff_DTO data);

        

    }
}
