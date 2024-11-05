using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Reports
{
    [Route("api/[controller]")]
    public class Naac_HSU_CR6ReportController : Controller
    {


        Naac_HSU_CR6ReportDelegate del = new Naac_HSU_CR6ReportDelegate();

        [Route("loaddata/{id:int}")]
        public Naac_HSU_CR6Report_DTO loaddata(int id)
        {
            Naac_HSU_CR6Report_DTO data = new Naac_HSU_CR6Report_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("HSUEGovernance623Report")]
        public Naac_HSU_CR6Report_DTO HSUEGovernance623Report([FromBody]Naac_HSU_CR6Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUEGovernance623Report(data);
        }


        [Route("HSUFinancialSupport632Report")]
        public Naac_HSU_CR6Report_DTO HSUFinancialSupport632Report([FromBody]Naac_HSU_CR6Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUFinancialSupport632Report(data);
        }


        [Route("HSUDevPrograms633Report")]
        public Naac_HSU_CR6Report_DTO HSUDevPrograms633Report([FromBody]Naac_HSU_CR6Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUDevPrograms633Report(data);
        }



        [Route("HSUDevPrograms634Report")]
        public Naac_HSU_CR6Report_DTO HSUDevPrograms634Report([FromBody]Naac_HSU_CR6Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUDevPrograms634Report(data);
        }

        [Route("HSUGovtFunding642Report")]
        public Naac_HSU_CR6Report_DTO HSUGovtFunding642Report([FromBody]Naac_HSU_CR6Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUGovtFunding642Report(data);
        }



        [Route("HSUQualityAssurance652Report")]
        public Naac_HSU_CR6Report_DTO HSUQualityAssurance652Report([FromBody]Naac_HSU_CR6Report_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.HSUQualityAssurance652Report(data);
        }



    }
}
