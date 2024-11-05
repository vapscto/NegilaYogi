using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Chairman.Interfaces
{
   public interface Clg_ClassDetailsInterface
    {

        Clg_ClassDetails_DTO loaddata(Clg_ClassDetails_DTO data);
        Clg_ClassDetails_DTO getcourse(Clg_ClassDetails_DTO data);
        Clg_ClassDetails_DTO report(Clg_ClassDetails_DTO data);

    }
}
