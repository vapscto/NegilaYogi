using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGStudentRouteMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGStudentRouteMappingDTO, CLGStudentRouteMappingDTO> _areazone = new CommonDelegate<CLGStudentRouteMappingDTO, CLGStudentRouteMappingDTO>();

        public CLGStudentRouteMappingDTO getdata(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/getdata/");
        }
        public CLGStudentRouteMappingDTO savedata(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/savedata/");
        }
        public CLGStudentRouteMappingDTO geteditdata(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/geteditdata/");
        }
        public CLGStudentRouteMappingDTO getstudents(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/getstudents/");
        }
        public CLGStudentRouteMappingDTO checkduplicateno(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/checkduplicateno/");
        }
        public CLGStudentRouteMappingDTO viewrecordspopup(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/viewrecordspopup/");
        }
        public CLGStudentRouteMappingDTO getreportedit(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/getreportedit/");
        }
        public CLGStudentRouteMappingDTO getreporteditbuspass(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/getreporteditbuspass/");
        }
        public CLGStudentRouteMappingDTO savedatabuspass(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/savedatabuspass/");
        }
        
        public CLGStudentRouteMappingDTO deactivate(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/deactivate/");
        }
        public CLGStudentRouteMappingDTO SearchByColumn(CLGStudentRouteMappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGStudentRouteMappingFacade/SearchByColumn/");
        }
    }

}
