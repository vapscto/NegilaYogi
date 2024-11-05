using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class HolidayReportFacade : Controller
    {
        HolidayReportInterface _msthldy;
        public HolidayReportFacade(HolidayReportInterface mhi)
        {
            _msthldy = mhi;
        }
        // GET: api/values


        [HttpGet("{id:int}")]
        [Route("getdata/{id:int}")]
        public MasterHolidayDTO getdata(int id)
        {
            return _msthldy.getdata(id);
        }
        // POST api/values
        [HttpPost]
        [Route("Report")]
        public MasterHolidayDTO HolidayReport([FromBody]MasterHolidayDTO report)
        {
            return _msthldy.ReportList(report);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
