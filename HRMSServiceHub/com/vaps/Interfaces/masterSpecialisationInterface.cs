using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface masterSpecialisationInterface
    {
        masterSpecialisationDTO loaddata(masterSpecialisationDTO data);
        masterSpecialisationDTO savedata(masterSpecialisationDTO data);
        masterSpecialisationDTO EditData(masterSpecialisationDTO data);
        masterSpecialisationDTO masterDecative(masterSpecialisationDTO data);
    }
}
