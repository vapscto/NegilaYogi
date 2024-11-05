using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
    public interface HOD_PRINCInterface
    {
        HOD_DTO getdata(HOD_DTO data);
        HOD_DTO savedata(HOD_DTO data);
        HOD_DTO mappHODdata(HOD_DTO data);
        HOD_DTO updateHOD(HOD_DTO data);
        HOD_DTO deactiveY(HOD_DTO data);

    }
}


