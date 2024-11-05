using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class VehicalDriverMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VehicalDriverMappingDTO, VehicalDriverMappingDTO> comml = new CommonDelegate<VehicalDriverMappingDTO, VehicalDriverMappingDTO>();

        public VehicalDriverMappingDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "VehicalDriverMappingFacadeControlling/getdata/");
        }


        public VehicalDriverMappingDTO savedata(VehicalDriverMappingDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalDriverMappingFacadeControlling/savedata/");
        }

        public VehicalDriverMappingDTO activedeactive(VehicalDriverMappingDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalDriverMappingFacadeControlling/activedeactive/");
        }


        public VehicalDriverMappingDTO editdata(VehicalDriverMappingDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalDriverMappingFacadeControlling/editdata/");
        }
    }
}
