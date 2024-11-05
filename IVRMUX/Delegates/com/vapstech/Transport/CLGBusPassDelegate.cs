using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGBusPassDelegate
    {
        CommonDelegate<CLGBusPassDTO, CLGBusPassDTO> _comm = new CommonDelegate<CLGBusPassDTO, CLGBusPassDTO>();

        public CLGBusPassDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "CLGBusPassFacade/getdata/");
        }
        public CLGBusPassDTO searchdetails(CLGBusPassDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGBusPassFacade/searchdetails/");
        }
        public CLGBusPassDTO showmodaldetails(CLGBusPassDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGBusPassFacade/showmodaldetails/");
        }
        public CLGBusPassDTO savelist(CLGBusPassDTO data)
        {
            return _comm.POSTDataTransport(data, "CLGBusPassFacade/savelist/");
        }

    }
}
