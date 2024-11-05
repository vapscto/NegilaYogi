using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface ECRInterface
    {
        ECRDTO getBasicData(ECRDTO dto);
        //Task<ECRDTO> getEmployeedetailsBySelection(ECRDTO dto);
        ECRDTO getEmployeedetailsBySelection(ECRDTO dto);
        ECRDTO get_depts(ECRDTO dto);
        ECRDTO get_desig(ECRDTO dto);
        ECRDTO SaveData(ECRDTO data);
        ECRDTO GetEmpDetails(ECRDTO data);
    }
}
