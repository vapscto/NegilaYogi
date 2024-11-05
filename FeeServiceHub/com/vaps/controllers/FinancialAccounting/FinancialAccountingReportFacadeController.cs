using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces.FinancialAccounting;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers.FinancialAccounting
{
    [Route("api/[controller]")]
    public class FinancialAccountingReportFacadeController : Controller
    {
        public FinancialAccountingReportInterface _feemaster;

        public FinancialAccountingReportFacadeController(FinancialAccountingReportInterface feemaster)
        {
            _feemaster = feemaster;
        }

        [Route("GetInitialData")]
        public FinancialAccountingReportDTO GetInitialData([FromBody] FinancialAccountingReportDTO data)
        {
            return _feemaster.GetInitialData(data);
        }

        [HttpPost]
        [Route("getReport")]
        public FinancialAccountingReportDTO getReport([FromBody]FinancialAccountingReportDTO data)
        {
            return _feemaster.getReport(data);
        }
        [HttpPost]
        [Route("subreport")]
        public FinancialAccountingReportDTO subreport([FromBody]FinancialAccountingReportDTO data)
        {
            return _feemaster.subreport(data);
        }


    }
}
