using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface RTODetailsInterface
    {
        RTODetailsDTO getdata(int id);
        RTODetailsDTO savedata(RTODetailsDTO data);
        RTODetailsDTO edit(RTODetailsDTO data);
        RTODetailsDTO Onvahiclechange(RTODetailsDTO data);
        RTODetailsDTO deleterecord(RTODetailsDTO data);

        


    }
}
