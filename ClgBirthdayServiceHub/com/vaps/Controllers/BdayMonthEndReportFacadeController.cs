using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;
using ClgBirthdayServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.BirthDay;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClgBirthdayServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class BdayMonthEndReportFacadeController : Controller
    {

        public BdayMonthEndReportInterface _clgbirthday;
        public BdayMonthEndReportFacadeController(BdayMonthEndReportInterface clgbirthday)
        {
            _clgbirthday = clgbirthday;
        }
        [Route("getloaddata")]
        public ClgBirthDayDTO getloaddata([FromBody] ClgBirthDayDTO data)
        {
            return _clgbirthday.getloaddata(data);
        }
        [Route("getmonthreport")]
        public Task<ClgBirthDayDTO> getmonthreport([FromBody] ClgBirthDayDTO data)
        {
            return _clgbirthday.getmonthreport(data);
        }




    }
}
