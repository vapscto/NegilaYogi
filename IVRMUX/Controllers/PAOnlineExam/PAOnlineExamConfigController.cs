using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.OnlineExam;
using PreadmissionDTOs.PAOnlineExam;
using IVRMUX.Delegates.PAOnlineExam;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.PAOnlineExam
{
    [Route("api/[controller]")]
    public class PAOnlineExamConfigController : Controller
    {
        PAOnlineExamConfigDelegate oed = new PAOnlineExamConfigDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }       
 

        [Route("getloaddata/{id:int}")]
        public PAOnlineExamConfigDTO getloaddata(int id)
        {
            PAOnlineExamConfigDTO data = new PAOnlineExamConfigDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.getloaddata(data);
        }
        //----------------------------1stTab
        [Route("savedata")]
        public PAOnlineExamConfigDTO savedata([FromBody] PAOnlineExamConfigDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.savedata(data);
        }
        [Route("editQuestion")]
        public PAOnlineExamConfigDTO editQuestion([FromBody] PAOnlineExamConfigDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.editQuestion(data);
        }
        [Route("Deletedetails")]
        public PAOnlineExamConfigDTO Deletedetails([FromBody] PAOnlineExamConfigDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.Deletedetails(data);
        }

        [Route("getreport")]
        public PAOnlineExamConfigDTO getreport([FromBody] PAOnlineExamConfigDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return oed.getreport(data);
        }
    }
}
