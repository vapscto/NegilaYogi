using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class GatePassController : Controller
    {
        GatePassDelegate delobj = new GatePassDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getDetails/{id:int}")]
        public GatePassDTO getDetails(int id)
        {
            GatePassDTO dto = new GatePassDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return delobj.getDetails(dto);
        }
        // POST api/<controller>
        [HttpPost]
        [Route("saveData")]
        public GatePassDTO saveData([FromBody]GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.saveData(data);
        }

        [Route("getStudentDetails")]
        public GatePassDTO getStudentDetails([FromBody]GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.getStudentDetails(data);
        }

        [Route("sendmail")]
        public GatePassDTO sendmail([FromBody]GatePassDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.sendmail(data);
        }

    }
}
