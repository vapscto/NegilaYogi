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
    public class OnlineLeaveApplicationFacade : Controller
    {
        public OnlineLeaveApplicationInterface _leave;
        public OnlineLeaveApplicationFacade(OnlineLeaveApplicationInterface _leavec)
        {
            _leave = _leavec;
        }
      

        [Route("getonlineLeave")]
        public LeaveCreditDTO getonlineLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.getonlineLeave(data);
        }
   
        [Route("save")]
        public LeaveCreditDTO saveLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.saveonlineLeave(data);
        }

        [Route("getonlineLeavestatus")]
        public LeaveCreditDTO getonlineLeavestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getonlineLeavestatus(data);
        }

        [Route("saveadminLeave")]
        public LeaveCreditDTO saveadminLeave([FromBody] LeaveCreditDTO data)
        {
            return _leave.saveadminLeave(data);
        }

        [Route("getemployeeadmin")]
        public LeaveCreditDTO getemployeeadmin([FromBody] LeaveCreditDTO data)
        {
            return _leave.getemployeeadmin(data);
        }

        [Route("getSingleEmpLeavestatus")]
        public LeaveCreditDTO getSingleEmpLeavestatus([FromBody] LeaveCreditDTO data)
        {
            return _leave.getSingleEmpLeavestatus(data);
        }


        [Route("deactivateRecordById")]
        public LeaveCreditDTO deactivateRecordById([FromBody]LeaveCreditDTO dto)
        {
            return _leave.deactivate(dto);
        }
        [Route("requestleave")]
        public LeaveCreditDTO requestleave([FromBody]LeaveCreditDTO dto)
        {
            return _leave.requestleave(dto);
        }
        //-////////////////////////////////periodwiseleave////////////////////////////////////////////////////////

        [Route("getdetails")]
        public LeaveCreditDTO getdetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.getdetails(data);
        }
        [Route("getabsentstaff")]
        public LeaveCreditDTO getabsentstaff([FromBody] LeaveCreditDTO data)
        {
            return _leave.getabsentstaff(data);
        }
        [Route("get_free_stfdets")]
        public LeaveCreditDTO get_free_stfdets([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_free_stfdets(data);
        }
        [Route("get_period_alloted")]
        public LeaveCreditDTO get_period_alloted([FromBody] LeaveCreditDTO data)
        {
            return _leave.get_period_alloted(data);
        }
        [Route("savedetails")]
        public LeaveCreditDTO savedetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.savedetails(data);
        }
        [Route("updatedetails")]
        public LeaveCreditDTO updatedetails([FromBody] LeaveCreditDTO data)
        {
            return _leave.updatedetails(data);
        }        
    }
}
