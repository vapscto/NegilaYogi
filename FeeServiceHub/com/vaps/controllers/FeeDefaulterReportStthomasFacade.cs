using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeDefaulterReportStthomasFacade : Controller
    {
        public FeeDefaulterReportStthomasInterface _feetar;

        public FeeDefaulterReportStthomasFacade(FeeDefaulterReportStthomasInterface maspag)
        {
            _feetar = maspag;
        }

        [Route("getreport")]
        public FeeDefaulterReportStthomasDTO getreport([FromBody] FeeDefaulterReportStthomasDTO data)
        {
            return _feetar.getreport(data);
        }
    }
}
