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
    public class CLGTTCourseWiseReportDelegate
    {


        CommonDelegate<CLGTTCourseWiseReportDTO, CLGTTCourseWiseReportDTO> _comm = new CommonDelegate<CLGTTCourseWiseReportDTO, CLGTTCourseWiseReportDTO>();


        public CLGTTCourseWiseReportDTO getrpt(CLGTTCourseWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseWiseReportFacade/getrpt/");
        }
        public CLGTTCourseWiseReportDTO getdetails(CLGTTCourseWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseWiseReportFacade/getdetails/");
        }
        public CLGTTCourseWiseReportDTO getclass_catg(CLGTTCourseWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseWiseReportFacade/getclass_catg/");
        }
        public CLGTTCourseWiseReportDTO GetStudentDetails(CLGTTCourseWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTCourseWiseReportFacade/GetStudentDetails/");
        }
    }
}
