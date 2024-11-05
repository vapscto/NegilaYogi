using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface CLGBusRoutesDetailsInterface
    {
        CLGBusRoutesDetailsDTO loaddata(CLGBusRoutesDetailsDTO data);
        CLGBusRoutesDetailsDTO getbranch(CLGBusRoutesDetailsDTO data);
        CLGBusRoutesDetailsDTO getsemester(CLGBusRoutesDetailsDTO data);
  
        CLGBusRoutesDetailsDTO getreport(CLGBusRoutesDetailsDTO data);
    }
}
