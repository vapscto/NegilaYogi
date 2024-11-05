using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{

    [Route("api/[controller]")]
    public class LeaveCreditFacade : Controller
    {
        public LeaveCreditInterface _leave;
        public LeaveCreditFacade(LeaveCreditInterface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("GetLeaveCredit")]
        public LeaveCreditDTO GetLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.getleave(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_departments(data);
        }
        [HttpPost]
        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_designation(data);
        }
        [HttpPost]
        [Route("get_grade")]
        public LeaveCreditDTO get_grade([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_grade(data);
        }


        [HttpPost]
        [Route("get_leavecode")]
        public LeaveCreditDTO get_leavecode([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_leavecode(data);
        }
        [HttpPost]
        public LeaveCreditDTO Post([FromBody] LeaveCreditDTO data)
        {
            // return _reg.getregdata(reg);
            return  _leave.SaveData(data);
        }

    }
}
