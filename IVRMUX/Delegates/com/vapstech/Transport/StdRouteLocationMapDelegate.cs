using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class StdRouteLocationMapDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<StdRouteLocationMapDTO, StdRouteLocationMapDTO> comml = new CommonDelegate<StdRouteLocationMapDTO, StdRouteLocationMapDTO>();

        public StdRouteLocationMapDTO Getreportdetails(StdRouteLocationMapDTO data)
        {
            return comml.POSTDataTransport(data, "StdRouteLocationMapFacade/Getreportdetails/");
        }
        public StdRouteLocationMapDTO on_pic_route_change(StdRouteLocationMapDTO data)
        {
            return comml.POSTDataTransport(data, "StdRouteLocationMapFacade/on_pic_route_change/");
        }
        public StdRouteLocationMapDTO getdata(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/getdata/");
        }

        public StdRouteLocationMapDTO savedata(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/savedata/");
        }

        public StdRouteLocationMapDTO check_feegroup(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/check_feegroup/");
        }

        public StdRouteLocationMapDTO deactivate(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/deactivate/");
        }


        
        public StdRouteLocationMapDTO get_cls_secs(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/get_cls_secs/");
        }
        public StdRouteLocationMapDTO get_sections(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/get_sections/");
        }
        public StdRouteLocationMapDTO getreport(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/getreport/");
        }


        public StdRouteLocationMapDTO getreportedit(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/getreportedit/");
        }
        public StdRouteLocationMapDTO get_data(StdRouteLocationMapDTO id)
        {
            return comml.POSTDataTransport(id, "StdRouteLocationMapFacade/get_data/");
        }


    }
}
