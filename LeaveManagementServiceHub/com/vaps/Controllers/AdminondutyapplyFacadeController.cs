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
    public class AdminondutyapplyFacadeController : Controller
    {

        public AdminondutyapplyInterface _lmContext;
        public AdminondutyapplyFacadeController(AdminondutyapplyInterface _Int)
        {
            _lmContext = _Int;
        }

        [Route("GetData")]
        public AdminondutyapplyDTO Getdetails([FromBody] AdminondutyapplyDTO data)
        {
            return _lmContext.getdata(data);
        }
        [Route("employeedetails")]
        public AdminondutyapplyDTO employeedetails([FromBody] AdminondutyapplyDTO data)
        {
            return _lmContext.employeedetails(data);
        }
        [Route("requestleave")]
        public AdminondutyapplyDTO requestleave([FromBody] AdminondutyapplyDTO data)
        {
            return _lmContext.requestleave(data);
        }
        [Route("viewcomment")]
        public AdminondutyapplyDTO viewcomment([FromBody] AdminondutyapplyDTO data)
        {
            return _lmContext.viewcomment(data);
        }
        [Route("ActiveDeactiveRecord")]
        public AdminondutyapplyDTO ActiveDeactiveRecord([FromBody] AdminondutyapplyDTO data)
        {
            return _lmContext.ActiveDeactiveRecord(data);
        }
        [Route("editData")]
        public AdminondutyapplyDTO editData([FromBody] AdminondutyapplyDTO data)
        {
            return _lmContext.editData(data);
        }
    }
}

