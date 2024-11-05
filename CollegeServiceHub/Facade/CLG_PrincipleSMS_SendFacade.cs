using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CLG_PrincipleSMS_SendFacade : Controller
    {
        public CLG_PrincipleSMS_SendInterface _SendSms;

        public CLG_PrincipleSMS_SendFacade(CLG_PrincipleSMS_SendInterface data)
        {
            _SendSms = data;
        }
        [Route("Getdetails")]
        public async Task<SendSMSDTO> Getdetails([FromBody] SendSMSDTO data)//int IVRMM_Id
        {
            return await _SendSms.Getdetails(data);
        }
        [Route("savedetail")]
        public async Task<SendSMSDTO> savedetail([FromBody] SendSMSDTO data)
        {
            return await _SendSms.savedetail(data);
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
        [Route("getCourse")]
        public SendSMSDTO getCourse([FromBody]SendSMSDTO data)
        {
            return _SendSms.getCourse(data);
        }
        [Route("getBranch")]
        public SendSMSDTO getBranch([FromBody]SendSMSDTO data)
        {
            return _SendSms.getBranch(data);
        }
        [Route("getSemister")]
        public SendSMSDTO getSemister([FromBody]SendSMSDTO data)
        {
            return _SendSms.getSemister(data);
        }
    }
}
