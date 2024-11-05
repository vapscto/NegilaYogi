using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Admission
{
    [Route("api/[controller]")]
    public class CollegeStudenttctransactionController : Controller
    {
        public CollegeStudenttctransactionDelegate _delg = new CollegeStudenttctransactionDelegate();
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
        [Route("loaddata/{id:int}")]
        public CollegeStudenttctransactionDTO loaddata(int id)
        {
            CollegeStudenttctransactionDTO data = new CollegeStudenttctransactionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }

        [Route("onchangeyear")]
        public CollegeStudenttctransactionDTO onchangeyear([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeStudenttctransactionDTO onchangecourse([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStudenttctransactionDTO onchangebranch([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeStudenttctransactionDTO onchangesemester([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesemester(data);
        }
        [Route("onchangesection")]
        public CollegeStudenttctransactionDTO onchangesection([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangesection(data);
        }

        [Route("searchfilter")]
        public CollegeStudenttctransactionDTO searchfilter([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.searchfilter(data);
        }

        [Route("onchangestudent")]
        public CollegeStudenttctransactionDTO onchangestudent([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.onchangestudent(data);
        }
        [Route("chk_dup_tc")]
        public CollegeStudenttctransactionDTO chk_dup_tc([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.chk_dup_tc(data);
        }

        [Route("savetc")]
        public CollegeStudenttctransactionDTO savetc([FromBody]CollegeStudenttctransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.savetc(data);
        }
    }
}
