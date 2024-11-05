using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using Recruitment.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class TrainingAuthorizationFacadeController : Controller
    {
        public TrainingAuthorizationInterface _leave;
        public TrainingAuthorizationFacadeController(TrainingAuthorizationInterface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("getAuthLeave")]
        public LeaveCreditDTO getAuthLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.getAuthLeave(data);
        }
        [Route("saveauthdata")]
        public LeaveCreditDTO saveauthdata([FromBody] LeaveCreditDTO data)
        {
            return _leave.saveauthdata(data);
        }
        [Route("getauthdata")]
        public LeaveCreditDTO getauthdata([FromBody] LeaveCreditDTO data)
        {
            return _leave.getauthdata(data);
        }
        [Route("editdetails/{id:int}")]
        public LeaveCreditDTO editdetails(int id)
        {
            return _leave.editdetails(id);
        }

        [Route("deleteauth")]
        public LeaveCreditDTO deleteauth([FromBody] LeaveCreditDTO id)
        {
            return _leave.deleteauth(id);
        }

        [Route("getemployeelist")]
        [HttpPost]
        public LeaveCreditDTO getemployeelist([FromBody]LeaveCreditDTO data)
        {
            return _leave.getemployeelist(data);
        }
    }
}


