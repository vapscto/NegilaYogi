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
    public class CLGTTStaffWiseReportDelegate
    {

        CommonDelegate<CLGTTStaffWiseReportDTO, CLGTTStaffWiseReportDTO> _comm = new CommonDelegate<CLGTTStaffWiseReportDTO, CLGTTStaffWiseReportDTO>();

        public CLGTTStaffWiseReportDTO getrpt(CLGTTStaffWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTStaffWiseReportFacade/getrpt/");
        }
        public CLGTTStaffWiseReportDTO getdetails(CLGTTStaffWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTStaffWiseReportFacade/getdetails/");
        }
        public CLGTTStaffWiseReportDTO GetStaffDetails(CLGTTStaffWiseReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGTTStaffWiseReportFacade/GetStaffDetails/");
        }
    }
}
