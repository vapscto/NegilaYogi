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
    public class FeeDueDateReportFacade : Controller
    {
        public FeeDueDateReportInterface _IStatus;

        public FeeDueDateReportFacade(FeeDueDateReportInterface IStatus)
        {
            _IStatus = IStatus;

        }
        [HttpPost]
        [Route("getinitialdata")]
        public FeeDueDateReportDTO getInitialData([FromBody] FeeDueDateReportDTO data)
        {
            return _IStatus.getInitailData(data);
        }

        [HttpPost]
        public Task<FeeDueDateReportDTO> SearchData([FromBody] FeeDueDateReportDTO Clscatag)
        {
            return _IStatus.SearchData(Clscatag);
        }

        [HttpPost]
        [Route("getdata")]
        public FeeDueDateReportDTO getdata([FromBody] FeeDueDateReportDTO data)
        {
            return _IStatus.getdata(data);
        }

        //Income Report
        [Route("getreport")]
        public Task<FeeDueDateReportDTO> getreport([FromBody]FeeDueDateReportDTO data)
        {
            return _IStatus.getreport(data);
        }
    }
}
