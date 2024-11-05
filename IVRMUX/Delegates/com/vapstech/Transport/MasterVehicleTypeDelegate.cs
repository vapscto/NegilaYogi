using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class MasterVehicleTypeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<MasterVehicleTypeDTO, MasterVehicleTypeDTO> _areazone = new CommonDelegate<MasterVehicleTypeDTO, MasterVehicleTypeDTO>();
        public MasterVehicleTypeDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "MasterVehicleTypeFacade/getdata/");
        }
        public MasterVehicleTypeDTO savedata(MasterVehicleTypeDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterVehicleTypeFacade/savedata/");
        }
        public MasterVehicleTypeDTO geteditdata(MasterVehicleTypeDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterVehicleTypeFacade/geteditdata/");
        }
        public MasterVehicleTypeDTO activedeactive(MasterVehicleTypeDTO data)
        {
            return _areazone.POSTDataTransport(data, "MasterVehicleTypeFacade/activedeactive/");
        }
    }
}