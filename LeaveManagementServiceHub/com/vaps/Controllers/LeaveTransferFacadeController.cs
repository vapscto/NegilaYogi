using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LeaveTransferFacadeController : Controller
    {
       // public MasterLeaveYearInterface _ads;
        public LeaveTransferInterface _leave;
        public LeaveTransferFacadeController(LeaveTransferInterface _leavec)
        {
            _leave = _leavec;
        }

      


        [HttpPost]
        [Route("getLeaveOB")]
        public LeaveCreditDTO getLeaveOB([FromBody] LeaveCreditDTO data)
        {
            return _leave.getLeaveOB(data);
        }

        [Route("onloadgetdetails")]
        public LeaveCreditDTO getinitialdata([FromBody]LeaveCreditDTO dto)
        {
            return _leave.getBasicData(dto);
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


        [HttpPost]
        [Route("SaveDetails")]
        public LeaveCreditDTO SaveDetails([FromBody]LeaveCreditDTO data)
        {
            return _leave.SaveDetails(data);
        }

        [HttpPost]
        [Route("SaveDetails11")]
        public LeaveCreditDTO SaveDetails11([FromBody]LeaveCreditDTO data)
        {
            return _leave.SaveDetails11(data);
        }

        [HttpPost]
        [Route("leavecarryforward")]
        public LeaveCreditDTO leavecarryforward([FromBody]LeaveCreditDTO data)
        {
            return _leave.leavecarryforward(data);
        }


        [Route("deletepages/{id:int}")]
        public LeaveCreditDTO deletepages(int id)
        {
            return _leave.deletepages(id);
        }

    }

}
