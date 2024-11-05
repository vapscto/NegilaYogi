using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Employee.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Employee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Employee.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeProfileUpdateApprovalFacadeController : Controller
    {
        public EmployeeProfileUpdateApprovalInterface _interface;

        public EmployeeProfileUpdateApprovalFacadeController(EmployeeProfileUpdateApprovalInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
         
        [Route("loaddataprofileupdate")]
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdate([FromBody] EmployeeProfileUpdateApprovalDTO data)
        {
            return _interface.loaddataprofileupdate(data);
        }

        [Route("Getcastecatgory")]
        public EmployeeProfileUpdateApprovalDTO Getcastecatgory([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {           
            return _interface.Getcastecatgory(data);
        }

        [Route("Getcaste")]
        public EmployeeProfileUpdateApprovalDTO Getcaste([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            return _interface.Getcaste(data);
        }

        [Route("SaveData")]
        public EmployeeProfileUpdateApprovalDTO SaveData([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            return _interface.SaveData(data);
        }

        //Approval Details

        [Route("loaddataprofileupdateapproval")]
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdateapproval([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            return _interface.loaddataprofileupdateapproval(data);
        }

        [Route("OnChangeOfEmployee")]
        public EmployeeProfileUpdateApprovalDTO OnChangeOfEmployee([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            return _interface.OnChangeOfEmployee(data);
        }

        [Route("SaveApprovedData")]
        public EmployeeProfileUpdateApprovalDTO SaveApprovedData([FromBody]  EmployeeProfileUpdateApprovalDTO data)
        {
            return _interface.SaveApprovedData(data);
        }
    }
}
