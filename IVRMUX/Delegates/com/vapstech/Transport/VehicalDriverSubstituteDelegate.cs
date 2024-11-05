using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class VehicalDriverSubstituteDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VehicalDriverSubstituteDTO, VehicalDriverSubstituteDTO> comml = new CommonDelegate<VehicalDriverSubstituteDTO, VehicalDriverSubstituteDTO>();

        public VehicalDriverSubstituteDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "VehicalDriverSubstituteFacade/getdata/");
        }
       


        public VehicalDriverSubstituteDTO savedata(VehicalDriverSubstituteDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalDriverSubstituteFacade/savedata/");
        }

        public VehicalDriverSubstituteDTO get_driver_data(VehicalDriverSubstituteDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalDriverSubstituteFacade/get_driver_data/");
        }


        public VehicalDriverSubstituteDTO editdata(VehicalDriverSubstituteDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalDriverSubstituteFacade/editdata/");
        }
    }
}
