using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegeStudenttctransactionFacadeController : Controller
    {
        public CollegeStudenttctransactionInterface _interface;

        public CollegeStudenttctransactionFacadeController(CollegeStudenttctransactionInterface _inter)
        {
            _interface = _inter;
        }

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

        [Route("loaddata")]
        public CollegeStudenttctransactionDTO loaddata([FromBody] CollegeStudenttctransactionDTO data)
        {           
            return _interface.loaddata(data);
        }

        [Route("onchangeyear")]
        public CollegeStudenttctransactionDTO onchangeyear([FromBody]CollegeStudenttctransactionDTO data)
        {          
            return _interface.onchangeyear(data);
        }
        [Route("onchangecourse")]
        public CollegeStudenttctransactionDTO onchangecourse([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.onchangecourse(data);
        }
        [Route("onchangebranch")]
        public CollegeStudenttctransactionDTO onchangebranch([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.onchangebranch(data);
        }
        [Route("onchangesemester")]
        public CollegeStudenttctransactionDTO onchangesemester([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.onchangesemester(data);
        }
        [Route("onchangesection")]
        public CollegeStudenttctransactionDTO onchangesection([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.onchangesection(data);
        }

        [Route("searchfilter")]
        public CollegeStudenttctransactionDTO searchfilter([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.searchfilter(data);
        }
        
        [Route("onchangestudent")]
        public CollegeStudenttctransactionDTO onchangestudent([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.onchangestudent(data);
        }
        [Route("chk_dup_tc")]
        public CollegeStudenttctransactionDTO chk_dup_tc([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.chk_dup_tc(data);
        }
        [Route("savetc")]
        public CollegeStudenttctransactionDTO savetc([FromBody]CollegeStudenttctransactionDTO data)
        {
            return _interface.savetc(data);
        }
    }
}
