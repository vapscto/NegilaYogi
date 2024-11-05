
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
    public class SendSMSFacadeController : Controller
    {
        public SendSMSInterface _SendSms;

        public SendSMSFacadeController(SendSMSInterface data)
        {
            _SendSms = data;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [Route("Getdetails")]
        public async Task<SendSMSDTO> Getdetails([FromBody] SendSMSDTO data)//int IVRMM_Id
        {           
            return await _SendSms.Getdetails(data);          
        }

     
        [Route("savedetail")]
        public async Task< SendSMSDTO> savedetail([FromBody] SendSMSDTO message)
        {
            return await _SendSms.savedetail(message);
        }
        
        [Route("Getdepartment")]
        public SendSMSDTO Getdepartment([FromBody]SendSMSDTO data)
        {
            return _SendSms.Getdepartment(data);
        }

        [Route("GetStudentDetails")]
        public async Task<SendSMSDTO> GetStudentDetails([FromBody] SendSMSDTO data)
        {
            return await _SendSms.GetStudentDetails(data);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]SendSMSDTO data)
        {
            return _SendSms.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public SendSMSDTO get_designation([FromBody]SendSMSDTO data)
        {
            return _SendSms.get_designation(data);
        }
        [Route("get_employee")]
        public SendSMSDTO get_employee([FromBody]SendSMSDTO data)
        {
            return _SendSms.get_employee(data);
        }
    }
    
}

