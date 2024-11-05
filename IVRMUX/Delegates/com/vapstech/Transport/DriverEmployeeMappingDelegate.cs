using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class DriverEmployeeMappingDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<DriverEmployeeMappingDTO, DriverEmployeeMappingDTO> comml = new CommonDelegate<DriverEmployeeMappingDTO, DriverEmployeeMappingDTO>();

        public DriverEmployeeMappingDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "DriverEmployeeMappingFacade/getdata/");
        }

        public DriverEmployeeMappingDTO savedata(DriverEmployeeMappingDTO data)
        {
            return comml.POSTDataTransport(data, "DriverEmployeeMappingFacade/savedata/");
        }


        public DriverEmployeeMappingDTO edit(DriverEmployeeMappingDTO data)
        {
            return comml.POSTDataTransport(data, "DriverEmployeeMappingFacade/edit/");
        }
        public DriverEmployeeMappingDTO deletedata(DriverEmployeeMappingDTO data)
        {
            return comml.POSTDataTransport(data, "DriverEmployeeMappingFacade/deletedata/");
        }


    }
}
