using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGTransportApproveDelegate
    {
        CommonDelegate<CLGTransportApproveDTO, CLGTransportApproveDTO> _comm = new CommonDelegate<CLGTransportApproveDTO, CLGTransportApproveDTO>();

        public CLGTransportApproveDTO getdata(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/getdata/");
        }
        public CLGTransportApproveDTO searchdetails(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/searchdetails/");
        }
        public CLGTransportApproveDTO gridaconchange(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/gridaconchange/");
        }
        public CLGTransportApproveDTO showmodaldetails(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/showmodaldetails/");
        }
        public CLGTransportApproveDTO savelist(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/savelist/");
        }
        public CLGTransportApproveDTO editapprove(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/editapprove/");
        }
        public CLGTransportApproveDTO CancelRejection(CLGTransportApproveDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGTransportApproveFacade/CancelRejection/");
        }
        

    }
}
