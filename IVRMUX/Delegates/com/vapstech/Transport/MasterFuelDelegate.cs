using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterFuelDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterFuelDTO, MasterFuelDTO> _areazone = new CommonDelegate<MasterFuelDTO, MasterFuelDTO>();


        public MasterFuelDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "MasterFuelFacade/getdata/");
        }
        public MasterFuelDTO savedata(MasterFuelDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterFuelFacade/savedata/");
        }
        public MasterFuelDTO geteditdata(MasterFuelDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterFuelFacade/geteditdata/");
        }
        public MasterFuelDTO activedeactive(MasterFuelDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterFuelFacade/activedeactive/");
        }
    }
}
