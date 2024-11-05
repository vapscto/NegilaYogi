using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Chirman;

namespace PortalHub.com.vaps.HOD.Interfaces
{
    public interface HODAttendanceDetailsInterface
    {
        ADMAttendenceDTO Getdetails(ADMAttendenceDTO data);
        ADMAttendenceDTO getclass(ADMAttendenceDTO data);
        ADMAttendenceDTO Getsection(ADMAttendenceDTO data);
        ADMAttendenceDTO GetAttendence(ADMAttendenceDTO data);
       Task<ADMAttendenceDTO>GetIndividualAttendenceAsync(ADMAttendenceDTO data);
    }
}
