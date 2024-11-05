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
    public class CLGTTSubjectWiseReportDelegate
    {
        CommonDelegate<CLGTTSubjectWiseReportDTO, CLGTTSubjectWiseReportDTO> comm = new CommonDelegate<CLGTTSubjectWiseReportDTO, CLGTTSubjectWiseReportDTO>();
        public CLGTTSubjectWiseReportDTO getdetails(CLGTTSubjectWiseReportDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTSubjectWiseReportFacade/getdetails");
        }
        public CLGTTSubjectWiseReportDTO getbranch(CLGTTSubjectWiseReportDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTSubjectWiseReportFacade/getbranch");
        }
        public CLGTTSubjectWiseReportDTO getsemister(CLGTTSubjectWiseReportDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTSubjectWiseReportFacade/getsemister");
        }
        public CLGTTSubjectWiseReportDTO savedetail(CLGTTSubjectWiseReportDTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGTTSubjectWiseReportFacade/savedetail");
        }

    }
}
