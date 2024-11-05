using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class General_SendSMSFacadeController : Controller
    {
        private General_SendSMSInterface _SendSms;
        public General_SendSMSFacadeController(General_SendSMSInterface obj)
        {
            _SendSms = obj;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("Getdetails")]
        public async Task<General_SendSMSDTO> Getdetails([FromBody] General_SendSMSDTO data)//int IVRMM_Id
        {
            return await _SendSms.Getdetails(data);
        }


        [Route("Getexam")]
        public General_SendSMSDTO Getexam([FromBody] General_SendSMSDTO data)//int IVRMM_Id
        {
            return  _SendSms.Getexam(data);
        }


        
        [Route("savedetail")]
        public async Task<General_SendSMSDTO> savedetail([FromBody] General_SendSMSDTO message)
        {
            return await _SendSms.savedetail(message);
        }

        [Route("Getdepartment")]
        public General_SendSMSDTO Getdepartment([FromBody]General_SendSMSDTO data)
        {
            return _SendSms.Getdepartment(data);
        }

        [Route("GetStudentDetails")]
        public async Task<General_SendSMSDTO> GetStudentDetails([FromBody] General_SendSMSDTO data)
        {
            return await _SendSms.GetStudentDetails(data);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public General_SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]General_SendSMSDTO data)
        {
            return _SendSms.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public General_SendSMSDTO get_designation([FromBody]General_SendSMSDTO data)
        {
            return _SendSms.get_designation(data);
        }
        [Route("get_employee")]
        public General_SendSMSDTO get_employee([FromBody]General_SendSMSDTO data)
        {
            return _SendSms.get_employee(data);
        }
        //
        [Route("SrkvsSerach")]
        public async Task<General_SendSMSDTO> SrkvsSerach([FromBody] General_SendSMSDTO data)
        {
            return await _SendSms.SrkvsSerach(data);
        }
    }
}
