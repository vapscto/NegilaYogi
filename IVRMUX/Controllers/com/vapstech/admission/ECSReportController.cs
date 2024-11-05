using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.admission
{
    [Route("api/[controller]")]
    public class ECSReportController : Controller
    {
        public ECSReportDelegate _delg = new ECSReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getloaddata/{id:int}")]
        public ECSReportDTO getloaddata(int id)
        {
            ECSReportDTO data = new ECSReportDTO();
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getloaddata(data);
        }

        [Route("getclass")]
        public ECSReportDTO getclass([FromBody] ECSReportDTO data)
        {         
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getclass(data);
        }
        [Route("getsection")]
        public ECSReportDTO getsection([FromBody] ECSReportDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getsection(data);
        }
        [Route("getreport")]
        public ECSReportDTO getreport([FromBody] ECSReportDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.getreport(data);
        }
        [Route("showecsdetails")]
        public ECSReportDTO showecsdetails([FromBody] ECSReportDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.showecsdetails(data);
        }
        [Route("searchByColumn")]
        public ECSReportDTO searchByColumn([FromBody] ECSReportDTO data)
        {
            data.MI_id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.searchByColumn(data);
        }
    }
}
