using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;
namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class VehicalRouteMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VehicalRouteMappingDTO, VehicalRouteMappingDTO> comml = new CommonDelegate<VehicalRouteMappingDTO, VehicalRouteMappingDTO>();

        public VehicalRouteMappingDTO getdata(int id)
        {
            return comml.GetDataByIdTransport(id, "VehicalRouteMappingFacade/getdata/");
        }

        public VehicalRouteMappingDTO savedata(VehicalRouteMappingDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalRouteMappingFacade/savedata/");
        }

        public VehicalRouteMappingDTO editdata(VehicalRouteMappingDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalRouteMappingFacade/editdata/");
        }


        public VehicalRouteMappingDTO activedeactive(VehicalRouteMappingDTO data)
        {
            return comml.POSTDataTransport(data, "VehicalRouteMappingFacade/activedeactive/");
        }

    }
}
