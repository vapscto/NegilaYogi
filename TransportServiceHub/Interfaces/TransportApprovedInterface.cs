using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface TransportApprovedInterface
    {
        TransportApprovedDTO getdata(TransportApprovedDTO id);
        TransportApprovedDTO searchdetails(TransportApprovedDTO data);
        TransportApprovedDTO gridaconchange(TransportApprovedDTO data);
        Task<TransportApprovedDTO> showmodaldetails(TransportApprovedDTO data);
        TransportApprovedDTO savelist(TransportApprovedDTO data);
        TransportApprovedDTO editapprove(TransportApprovedDTO data);
        TransportApprovedDTO CancelRejection(TransportApprovedDTO data);
        
    }
}
