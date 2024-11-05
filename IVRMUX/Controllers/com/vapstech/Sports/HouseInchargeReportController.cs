using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Sports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class HouseInchargeReportController : Controller
    {
        // GET: api/<controller>


       private HouseInchargeReportDelegate _objdel = new HouseInchargeReportDelegate();

       [Route("get_details/{id:int}")]
        public HouseInchargeReport_DTO get_details(int id)
        {
            HouseInchargeReport_DTO obj = new HouseInchargeReport_DTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_details(obj);
        }

        [Route("get_house")]
        public HouseInchargeReport_DTO get_house([FromBody] HouseInchargeReport_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_house(d);
        }

        [Route("get_reports")]
        public HouseInchargeReport_DTO get_reports([FromBody] HouseInchargeReport_DTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.get_reports(d);
        }


    }
}
