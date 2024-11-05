using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class CLGTRNCommonDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CLGTRNCommonDTO, CLGTRNCommonDTO> _areazone = new CommonDelegate<CLGTRNCommonDTO, CLGTRNCommonDTO>();

        public CLGTRNCommonDTO get_location(CLGTRNCommonDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGTRNCommonFacade/get_location/");
        }
        public CLGTRNCommonDTO get_section(CLGTRNCommonDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGTRNCommonFacade/get_section/");
        }
        public CLGTRNCommonDTO get_semister(CLGTRNCommonDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGTRNCommonFacade/get_semister/");
        }
        public CLGTRNCommonDTO getBranch(CLGTRNCommonDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGTRNCommonFacade/getBranch/");
        }
        public CLGTRNCommonDTO get_course(CLGTRNCommonDTO data)
        {
            return _areazone.POSTDataTransport(data, "CLGTRNCommonFacade/get_course/");
        }
    }

}
