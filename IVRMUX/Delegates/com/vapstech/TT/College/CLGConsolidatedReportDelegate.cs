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
    public class CLGConsolidatedReportDelegate
    {
        CommonDelegate<CLGConsolidatedReportDTO, CLGConsolidatedReportDTO> _comm = new CommonDelegate<CLGConsolidatedReportDTO, CLGConsolidatedReportDTO>();

        public CLGConsolidatedReportDTO getalldetails(CLGConsolidatedReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsolidatedReportFacade/getalldetails/");
        }
          public CLGConsolidatedReportDTO getrpt(CLGConsolidatedReportDTO data)
        {
            return _comm.POSTDataTimeTable(data, "CLGConsolidatedReportFacade/getrpt/");
        }
        

    }
}
