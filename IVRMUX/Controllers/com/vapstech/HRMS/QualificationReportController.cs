using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class QualificationReportController : Controller
    {
        QualificationReportDelegate del = new QualificationReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getalldetails/{id:int}")]
        public MasterEmployeeDTO getalldetails(int id)
        {
            MasterEmployeeDTO data = new MasterEmployeeDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetails(data);
        }
        [Route("getQualificationReport")]
        public MasterEmployeeDTO getQualificationReport([FromBody]MasterEmployeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getQualificationReport(data);
        }
    }
}
