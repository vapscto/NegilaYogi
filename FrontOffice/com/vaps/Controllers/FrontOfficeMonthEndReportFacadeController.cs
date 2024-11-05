using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;
using FrontOfficeHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FrontOfficeMonthEndReportFacadeController : Controller
    {

        public FrontOfficeMonthEndReportInterface _inter;
        public FrontOfficeMonthEndReportFacadeController(FrontOfficeMonthEndReportInterface brth)
        {
            _inter = brth;
        }

        [HttpGet("{id:int}")]
        public BirthDayDTO getdata(int id)
        {
            return _inter.getdata(id);
        }

        [Route("getmonthreport")]
        public BirthDayDTO getmonthreport([FromBody] BirthDayDTO obj)
        {
            return _inter.getmonthreport(obj);
        }

    }
}
