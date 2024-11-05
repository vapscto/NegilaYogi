using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class SMSGenaralFacadeController : Controller
    {
         SMSGenaralInterface _SendSms;

        public SMSGenaralFacadeController(SMSGenaralInterface SendSms)
        {
            _SendSms = SendSms;
        }
    
        [HttpPost]
        [Route("Getdetails")]
        public Task<SMSGenaralDTO> Getdetails([FromBody]SMSGenaralDTO mi_id)
        {
            return _SendSms.Getdetails(mi_id);
        }
        [Route("Getexam")]
        public SMSGenaralDTO Getexam([FromBody] SMSGenaralDTO data)//int IVRMM_Id
        {
            return _SendSms.Getexam(data);
        }



        [Route("savedetail")]
        public async Task<SMSGenaralDTO> savedetail([FromBody] SMSGenaralDTO message)
        {
            return await _SendSms.savedetail(message);
        }

        [Route("Getdepartment")]
        public SMSGenaralDTO Getdepartment([FromBody]SMSGenaralDTO data)
        {
            return _SendSms.Getdepartment(data);
        }

        [Route("GetStudentDetails")]
        public async Task<SMSGenaralDTO> GetStudentDetails([FromBody] SMSGenaralDTO data)
        {
            return await _SendSms.GetStudentDetails(data);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public SMSGenaralDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]SMSGenaralDTO data)
        {
            return _SendSms.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public SMSGenaralDTO get_designation([FromBody]SMSGenaralDTO data)
        {
            return _SendSms.get_designation(data);
        }
        [Route("get_employee")]
        public SMSGenaralDTO get_employee([FromBody]SMSGenaralDTO data)
        {
            return _SendSms.get_employee(data);
        }



    }
}
