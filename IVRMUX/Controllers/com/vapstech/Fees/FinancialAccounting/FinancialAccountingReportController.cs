using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Fees.FinancialAccounting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Fees.FinancialAccounting
{
    [Route("api/[controller]")]
    public class FinancialAccountingReportController : Controller
    {
        FinancialAccountingReportDelegate gate = new FinancialAccountingReportDelegate();


        [HttpGet]
        [Route("GetInitialData/{id:int}")]
        public FinancialAccountingReportDTO GetInitialData(int id)
        {
            FinancialAccountingReportDTO user = new FinancialAccountingReportDTO();
            user.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            user.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return gate.GetInitialData(user);
        }


        [HttpPost]
        [Route("getReport")]

        public FinancialAccountingReportDTO getReport([FromBody] FinancialAccountingReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.getReport(data);
        }

        [HttpPost]
        [Route("subreport")]

        public FinancialAccountingReportDTO subreport([FromBody] FinancialAccountingReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.subreport(data);
        }

    }
}
