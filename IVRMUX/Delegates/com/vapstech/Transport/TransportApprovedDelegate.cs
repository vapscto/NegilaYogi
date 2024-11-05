using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class TransportApprovedDelegate
    {
        CommonDelegate<TransportApprovedDTO, TransportApprovedDTO> _comm = new CommonDelegate<TransportApprovedDTO, TransportApprovedDTO>();

        public TransportApprovedDTO getdata(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/getdata/");
        }
        public TransportApprovedDTO searchdetails(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/searchdetails/");
        }
        public TransportApprovedDTO gridaconchange(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/gridaconchange/");
        }
        public TransportApprovedDTO showmodaldetails(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/showmodaldetails/");
        }
        public TransportApprovedDTO savelist(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/savelist/");
        }
        public TransportApprovedDTO editapprove(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/editapprove/");
        }
        public TransportApprovedDTO CancelRejection(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "TransportApprovedFacade/CancelRejection/");
        }
        

    }
}
