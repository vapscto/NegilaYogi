using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterLocationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterLocationDTO, MasterLocationDTO> comml = new CommonDelegate<MasterLocationDTO, MasterLocationDTO>();

        public MasterLocationDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "MasterLocationFacade/getdata/");
        }
        public MasterLocationDTO savedata(MasterLocationDTO data)
        {
            return comml.POSTDataTransport(data, "MasterLocationFacade/savedata/");
        }
        public MasterLocationDTO activedeactive(MasterLocationDTO data)
        {
            return comml.POSTDataTransport(data, "MasterLocationFacade/activedeactive/");
        }
        public MasterLocationDTO edit(MasterLocationDTO data)
        {
            return comml.POSTDataTransport(data, "MasterLocationFacade/edit/");
        }
    }
}
