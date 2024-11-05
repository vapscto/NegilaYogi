
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
    public class PushNotifyFacadeController : Controller
    {
        public PushNotifyInterface _SendSms;

        public PushNotifyFacadeController(PushNotifyInterface data)
        {
            _SendSms = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [Route("Getdetails")]
        public async Task<PushNotifyDTO> Getdetails([FromBody] PushNotifyDTO data)//int IVRMM_Id
        {           
            return await _SendSms.Getdetails(data);          
        }

     
        [Route("savedetail")]
        public async Task< PushNotifyDTO> savedetail([FromBody] PushNotifyDTO message)
        {
            return await _SendSms.savedetail(message);
        }
        
        [Route("Getdepartment")]
        public PushNotifyDTO Getdepartment([FromBody]PushNotifyDTO data)
        {
            return _SendSms.Getdepartment(data);
        }

        [Route("GetStudentDetails")]
        public async Task<PushNotifyDTO> GetStudentDetails([FromBody] PushNotifyDTO data)
        {
            return await _SendSms.GetStudentDetails(data);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public PushNotifyDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]PushNotifyDTO data)
        {
            return _SendSms.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public PushNotifyDTO get_designation([FromBody]PushNotifyDTO data)
        {
            return _SendSms.get_designation(data);
        }
        [Route("get_employee")]
        public PushNotifyDTO get_employee([FromBody]PushNotifyDTO data)
        {
            return _SendSms.get_employee(data);
        }
    }
    
}

