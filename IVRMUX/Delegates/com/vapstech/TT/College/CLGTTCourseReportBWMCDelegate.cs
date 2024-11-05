using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.TT
{
    public class CLGTTCourseReportBWMCDelegate
    {


        CommonDelegate<CLGTTCourseReportBWMCDTO, CLGTTCourseReportBWMCDTO> _comm = new CommonDelegate<CLGTTCourseReportBWMCDTO, CLGTTCourseReportBWMCDTO>();


        public CLGTTCourseReportBWMCDTO getrpt(CLGTTCourseReportBWMCDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseReportBWMCFacade/getrpt/");
        }
        public CLGTTCourseReportBWMCDTO getdetails(CLGTTCourseReportBWMCDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseReportBWMCFacade/getdetails/");
        }
        public CLGTTCourseReportBWMCDTO getclass_catg(CLGTTCourseReportBWMCDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseReportBWMCFacade/getclass_catg/");
        }

    
    }
}
