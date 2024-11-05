using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class MonthEndReportController : Controller
    {

      private MonthEndReportDelegate _delobj = new MonthEndReportDelegate();


        [Route("getdeatils/{id:int}")]
        public VisitorsMonthEndReport_DTO getdeatils(int id)
        {
            VisitorsMonthEndReport_DTO data = new VisitorsMonthEndReport_DTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.getdeatils(data);
        }

        [Route("get_month_report")]
        public VisitorsMonthEndReport_DTO get_month_report([FromBody]VisitorsMonthEndReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.GetReport(data);
        }



    }
}
