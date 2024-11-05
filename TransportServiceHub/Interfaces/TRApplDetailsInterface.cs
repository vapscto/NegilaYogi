using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
 public   interface TRApplDetailsInterface
    {
        TRApplDetailsDTO getdata(int id);
        TRApplDetailsDTO Getreportdetails(TRApplDetailsDTO data);
        TRApplDetailsDTO sendmsg(TRApplDetailsDTO data);
    }
}
