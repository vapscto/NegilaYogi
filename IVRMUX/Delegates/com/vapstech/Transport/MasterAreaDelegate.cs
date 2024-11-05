using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterAreaDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<MasterAreaDTO, MasterAreaDTO> _areazone = new CommonDelegate<MasterAreaDTO, MasterAreaDTO>();

        public MasterAreaDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "MasterAreaFacade/getdata/");
        }
        public MasterAreaDTO savedata(MasterAreaDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterAreaFacade/savedata/");
        }
        public MasterAreaDTO geteditdata(MasterAreaDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterAreaFacade/geteditdata/");
        }
        public MasterAreaDTO activedeactive(MasterAreaDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterAreaFacade/activedeactive/");
        }
    }

}
