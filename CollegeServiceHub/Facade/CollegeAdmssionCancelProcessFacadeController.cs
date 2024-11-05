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
    public class CollegeAdmssionCancelProcessFacadeController : Controller
    {
        private CollegeAdmssionCancelProcessInterface _intf;

        public CollegeAdmssionCancelProcessFacadeController(CollegeAdmssionCancelProcessInterface intf)
        {
            _intf = intf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getalldetails")]
        public CollegeAdmssionCancelProcessDTO getalldetails([FromBody]CollegeAdmssionCancelProcessDTO data)
        {
            return _intf.getalldetails(data);
        }
        [Route("onyearchange")]
        public CollegeAdmssionCancelProcessDTO onyearchange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            return _intf.onyearchange(id);
        }
        [Route("onCoursechange")]
        public CollegeAdmssionCancelProcessDTO onCoursechange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            return _intf.onCoursechange(id);
        }
        [Route("onBranchchange")]
        public CollegeAdmssionCancelProcessDTO onBranchchange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            return _intf.onBranchchange(id);
        }
        [Route("onSemchange")]
        public CollegeAdmssionCancelProcessDTO onSemchange([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            return _intf.onSemchange(id);
        }
        [Route("get_Studentdetails")]
        public CollegeAdmssionCancelProcessDTO get_Studentdetails([FromBody] CollegeAdmssionCancelProcessDTO id)
        {          
            return _intf.get_Studentdetails(id);
        }
        [Route("saveatt")]
        public CollegeAdmssionCancelProcessDTO saveatt([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            return _intf.saveatt(id);
        }
        [Route("getStudentdetails")]
        public CollegeAdmssionCancelProcessDTO getStudentdetails([FromBody] CollegeAdmssionCancelProcessDTO id)
        {
            return _intf.getStudentdetails(id);
        }
        
    }
}
