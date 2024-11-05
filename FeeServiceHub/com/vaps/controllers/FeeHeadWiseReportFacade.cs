using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeheadWiseReportFacade : Controller
    {
        public FeeHeadWiseReportInterface _IStatus;

        public FeeheadWiseReportFacade(FeeHeadWiseReportInterface IStatus)
        {
            _IStatus = IStatus;

        }


        // load initial dropdown

        [HttpPost]
        [Route("getinitialdata")]
        public FeeHeadWiseReportDTO getInitialData([FromBody] FeeHeadWiseReportDTO data)
        {
            return _IStatus.getInitailData(data);
        }

        [HttpPost]
        public FeeHeadWiseReportDTO SearchData([FromBody] FeeHeadWiseReportDTO Clscatag)
        {
            return _IStatus.SearchData(Clscatag);
        }

        [HttpPost]
        [Route("getdata")]
        public FeeHeadWiseReportDTO getdata([FromBody] FeeHeadWiseReportDTO data)
        {
            return _IStatus.getdata(data);
        }

        [HttpPost]
        [Route("getreport")]
        public FeeHeadWiseReportDTO getreport([FromBody] FeeHeadWiseReportDTO data)
        {
            return _IStatus.getreport(data);
        }

    }
}
