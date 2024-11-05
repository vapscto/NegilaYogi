using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class BuspassprintformDelegate
    {
        CommonDelegate<TransportApprovedDTO, TransportApprovedDTO> _comm = new CommonDelegate<TransportApprovedDTO, TransportApprovedDTO>();

        public TransportApprovedDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "BuspassprintformFacade/getdata/");
        }
        public TransportApprovedDTO searchdetails(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "BuspassprintformFacade/searchdetails/");
        }
        public TransportApprovedDTO showmodaldetails(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "BuspassprintformFacade/showmodaldetails/");
        }
        public TransportApprovedDTO savelist(TransportApprovedDTO data)
        {
            return _comm.POSTDataTransport(data, "BuspassprintformFacade/savelist/");
        }

    }
}
