using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface Ch_DatewiseAttendanceInterface
    {
        
        Ch_DatewiseAttendanceDTO Getdetails(Ch_DatewiseAttendanceDTO data);
        Ch_DatewiseAttendanceDTO getclass(Ch_DatewiseAttendanceDTO data);
        Ch_DatewiseAttendanceDTO Getsection(Ch_DatewiseAttendanceDTO data);
        Ch_DatewiseAttendanceDTO Getreport(Ch_DatewiseAttendanceDTO data);
    

    }
    
}
