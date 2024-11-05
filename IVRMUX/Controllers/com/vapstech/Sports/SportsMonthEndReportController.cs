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
    public class SportsMonthEndReportController : Controller
    {
        // GET: api/<controller>
        SportsMonthEndReportDelegate _delobj = new SportsMonthEndReportDelegate();

   
        [Route("getdeatils/{id:int}")]
        public SportsMonthEndReport_DTO getdeatils(int id)
        {
            SportsMonthEndReport_DTO data = new SportsMonthEndReport_DTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdeatils(data);
        }

        [Route("GetReport")]
        public SportsMonthEndReport_DTO GetReport([FromBody]SportsMonthEndReport_DTO data)
        {
           data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.GetReport(data);
        }

    }
}
