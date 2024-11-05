using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirthdayServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.BirthDay;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BirthdayServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClgBdayMonthEndReportFacadeController : Controller
    {
        public ClgBdayMonthEndReportInterface _clgbirthday;
        public ClgBdayMonthEndReportFacadeController(ClgBdayMonthEndReportInterface clgbirthday)
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
