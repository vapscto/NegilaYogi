using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
   public interface HODPaycareStaffDetailsInterface
    {
        HODPaycareStaffDetails_DTO Getdetails(HODPaycareStaffDetails_DTO data);

        HODPaycareStaffDetails_DTO Getemppop(HODPaycareStaffDetails_DTO data);

    }
}
