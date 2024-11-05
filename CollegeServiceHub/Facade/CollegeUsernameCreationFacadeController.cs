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
    public class CollegeUsernameCreationFacadeController : Controller
    {
        private CollegeUsernameCreationInterface _intf;

        public CollegeUsernameCreationFacadeController(CollegeUsernameCreationInterface intf)
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
        public CollegeUsernameCreationDTO getalldetails([FromBody]CollegeUsernameCreationDTO data)
        {
            return _intf.getalldetails(data);
        }
        [Route("onyearchange")]
        public CollegeUsernameCreationDTO onyearchange([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.onyearchange(id);
        }
        [Route("onCoursechange")]
        public CollegeUsernameCreationDTO onCoursechange([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.onCoursechange(id);
        }
        [Route("onBranchchange")]
        public CollegeUsernameCreationDTO onBranchchange([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.onBranchchange(id);
        }
        [Route("onSemchange")]
        public CollegeUsernameCreationDTO onSemchange([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.onSemchange(id);
        }
        [Route("get_Studentdetails")]
        public CollegeUsernameCreationDTO get_Studentdetails([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.get_Studentdetails(id);
        }
        [Route("saveatt")]
        public CollegeUsernameCreationDTO saveatt([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.saveatt(id);
        }
        [Route("getStudentusername")]
        public CollegeUsernameCreationDTO getStudentusername([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.getStudentusername(id);
        }
        [Route("SendSMS")]
        public Task<CollegeUsernameCreationDTO> SendSMS([FromBody] CollegeUsernameCreationDTO id)
        {
            return _intf.SendSMS(id);
        }
        


    }
}
