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
    public class LeaveTransactionManualFacade : Controller
    {
        public LeaveTransactionManualInterface _leave;
        public LeaveTransactionManualFacade(LeaveTransactionManualInterface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("getLeavetransm")]
        public LeaveCreditDTO getLeavetransm([FromBody] LeaveCreditDTO data)
        {
            return _leave.getLeavetransm(data);
        }
       
        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_departments(data);
        }
       
        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_designation(data);
        }
        [Route("get_employee")]
        public LeaveCreditDTO get_employee([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_employee(data);
        }
        [Route("get_Emp_lop")]
        public LeaveCreditDTO get_Emp_lop([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_Emp_lop(data);
        }
        [Route("saveDATA")]
        public LeaveCreditDTO saveDATA([FromBody] LeaveCreditDTO data)
        {
            return _leave.saveDATA(data);
        }
        [Route("Deletedetails")]
        public LeaveCreditDTO Deletedetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.Deletedetails(data);
        }
    }
}
