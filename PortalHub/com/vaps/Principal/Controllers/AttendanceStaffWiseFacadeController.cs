
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Principal.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;
//using AdmissionServiceHub.com.vaps.Interfaces;


using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Principal.Controllers
{
    [Route("api/[controller]")]
    public class AttendanceStaffWiseFacadeController : Controller
    {
        public AttendanceStaffWiseInterface _SendSms;

        public AttendanceStaffWiseFacadeController(AttendanceStaffWiseInterface data)
        {
            _SendSms = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
       
        [Route("Getdetails")]
        public AttendanceStaffWiseDTO Getdetails([FromBody] AttendanceStaffWiseDTO data)//int IVRMM_Id
        {           
            return _SendSms.Getdetails(data);          
        }

        [Route("Getdepartment")]
        public AttendanceStaffWiseDTO Getdepartment([FromBody]AttendanceStaffWiseDTO data)
        {
            return _SendSms.Getdepartment(data);
        }

        [Route("get_designation")]
        public AttendanceStaffWiseDTO get_designation([FromBody]AttendanceStaffWiseDTO data)
        {
            return _SendSms.get_designation(data);
        }
        [Route("get_department")]
        public AttendanceStaffWiseDTO get_department([FromBody]AttendanceStaffWiseDTO data)
        {
            return _SendSms.get_department(data);
        }
        [Route("get_employee")]
        public AttendanceStaffWiseDTO get_employee([FromBody]AttendanceStaffWiseDTO data)
        {
            return _SendSms.get_employee(data);
        }
    }
    
}

