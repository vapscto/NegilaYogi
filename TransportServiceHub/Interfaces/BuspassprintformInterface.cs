using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface BuspassprintformInterface
    {
        TransportApprovedDTO getdata(int id);
        TransportApprovedDTO searchdetails(TransportApprovedDTO data);
        Task<TransportApprovedDTO> showmodaldetails(TransportApprovedDTO data);
        TransportApprovedDTO savelist(TransportApprovedDTO data);
    }
}
