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
    public class LeaveOpeningBalanceFacade : Controller
    {
        LeaveOpeningBalanceInterface _leave;
        public LeaveOpeningBalanceFacade(LeaveOpeningBalanceInterface _leavec)
        {
            _leave = _leavec;
        }
       
        [Route("getLeaveOB")]
        public LeaveCreditDTO getLeaveOB([FromBody] LeaveCreditDTO data)
        {
            return _leave.getLeaveOB(data);
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

        [Route("get_Employe_ob")]
        public LeaveCreditDTO get_Employe_ob([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_Employe_ob(data);
        }
        [Route("get_ob_Details")]
        public LeaveCreditDTO get_ob_Details([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_ob_Details(data);
        }


      
        [Route("SaveDetails")]
        public LeaveCreditDTO SaveDetails([FromBody]LeaveCreditDTO obj)
        {
            return _leave.save(obj);
        }

        [Route("getpagedetails/{id:int}")]
        public LeaveCreditDTO getpagedetails(int id)
        {
            return _leave.getpagedetails(id);
        }

        [Route("deletepages/{id:int}")]
        public LeaveCreditDTO deletepages(int id)
        {
            return _leave.deletepages(id);
        }

    }
}
