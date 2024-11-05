using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
    public interface CLGTransportApproveInterface
    {
        CLGTransportApproveDTO getdata(CLGTransportApproveDTO id);
        CLGTransportApproveDTO searchdetails(CLGTransportApproveDTO data);
        CLGTransportApproveDTO gridaconchange(CLGTransportApproveDTO data);
        Task<CLGTransportApproveDTO> showmodaldetails(CLGTransportApproveDTO data);
        CLGTransportApproveDTO savelist(CLGTransportApproveDTO data);
        CLGTransportApproveDTO editapprove(CLGTransportApproveDTO data);
        CLGTransportApproveDTO CancelRejection(CLGTransportApproveDTO data);
        
    }
}
