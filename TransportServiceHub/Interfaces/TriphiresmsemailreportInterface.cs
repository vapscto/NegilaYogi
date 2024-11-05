using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
 public   interface TriphiresmsemailreportInterface
    {
        TriphiresmsemailreportDTO getdata(int id);
        TriphiresmsemailreportDTO Getreportdetails(TriphiresmsemailreportDTO data);
    }
}
