using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MsterSessionDelegate
    {
        CommonDelegate<MsterSessionDTO, MsterSessionDTO> _comm = new CommonDelegate<MsterSessionDTO, MsterSessionDTO>();
        public MsterSessionDTO getdata(int id)
        {
            return _comm.GetDataByIdTransport(id, "MsterSessionFacade/getdata/");
        }
        public MsterSessionDTO savedata(MsterSessionDTO data)
        {
            return _comm.POSTDataTransport(data, "MsterSessionFacade/savedata/");
        }
        public MsterSessionDTO edit(MsterSessionDTO data)
        {
            return _comm.POSTDataTransport(data, "MsterSessionFacade/edit/");
        }
        public MsterSessionDTO activedeactive(MsterSessionDTO data)
        {
            return _comm.POSTDataTransport(data, "MsterSessionFacade/activedeactive/");
        }
    }
}
