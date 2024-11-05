using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface RFIDDashboardInterface
    {
    
        RFIDDashboardDTO Getdetails(RFIDDashboardDTO id);
        RFIDDashboardDTO showstudentGrid(RFIDDashboardDTO id);
        RFIDDashboardDTO cleardata(RFIDDashboardDTO id);
      
    }
}
