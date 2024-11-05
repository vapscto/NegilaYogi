using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ADMAttendenceInterface
    {
        
        ADMAttendenceDTO Getdetails(ADMAttendenceDTO data);
        ADMAttendenceDTO getclass(ADMAttendenceDTO data);
        ADMAttendenceDTO Getsection(ADMAttendenceDTO data);
        ADMAttendenceDTO GetAttendence(ADMAttendenceDTO data);
        ADMAttendenceDTO GetIndividualAttendence(ADMAttendenceDTO data);

        
    }
}
