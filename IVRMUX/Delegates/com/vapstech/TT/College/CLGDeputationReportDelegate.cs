using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.TT
{
    public class CLGDeputationReportDelegate
    {

        CommonDelegate<CLGDeputationReportDTO, CLGDeputationReportDTO> _comm = new CommonDelegate<CLGDeputationReportDTO, CLGDeputationReportDTO>();

        public CLGDeputationReportDTO getdata(CLGDeputationReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGDeputationReportFacade/getdata/");
        }
        public CLGDeputationReportDTO getreport(CLGDeputationReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGDeputationReportFacade/getreport/");
        }
       
    }
}
