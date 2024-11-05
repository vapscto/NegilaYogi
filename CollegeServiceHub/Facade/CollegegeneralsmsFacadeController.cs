using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeServiceHub.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class CollegegeneralsmsFacadeController : Controller
    {
        CollegegeneralsmsInterface _SendSms;

        public CollegegeneralsmsFacadeController(CollegegeneralsmsInterface SendSms)
        {
            _SendSms = SendSms;
        }

        [HttpPost]
        [Route("Getdetails")]
        public Task<CollegegeneralsmsDTO> Getdetails([FromBody]CollegegeneralsmsDTO mi_id)
        {
            return _SendSms.Getdetails(mi_id);
        }
        [Route("Getexam")]
        public CollegegeneralsmsDTO Getexam([FromBody] CollegegeneralsmsDTO data)//int IVRMM_Id
        {
            return _SendSms.Getexam(data);
        }

        [Route("savedetail")]
        public async Task<CollegegeneralsmsDTO> savedetail([FromBody] CollegegeneralsmsDTO message)
        {
            return await _SendSms.savedetail(message);
        }

        [Route("Getdepartment")]
        public CollegegeneralsmsDTO Getdepartment([FromBody]CollegegeneralsmsDTO data)
        {
            return _SendSms.Getdepartment(data);
        }

        [Route("GetStudentDetails")]
        public async Task<CollegegeneralsmsDTO> GetStudentDetails([FromBody] CollegegeneralsmsDTO data)
        {
            return await _SendSms.GetStudentDetails(data);
        }

        [Route("GetEmployeeDetailsByLeaveYearAndMonth")]
        public CollegegeneralsmsDTO GetEmployeeDetailsByLeaveYearAndMonth([FromBody]CollegegeneralsmsDTO data)
        {
            return _SendSms.GetEmployeeDetailsByLeaveYearAndMonth(data);
        }
        [Route("get_designation")]
        public CollegegeneralsmsDTO get_designation([FromBody]CollegegeneralsmsDTO data)
        {
            return _SendSms.get_designation(data);
        }
        [Route("get_employee")]
        public CollegegeneralsmsDTO get_employee([FromBody]CollegegeneralsmsDTO data)
        {
            return _SendSms.get_employee(data);
        }

        [Route("onSelectyear")]
        public CollegegeneralsmsDTO onSelectyear([FromBody] CollegegeneralsmsDTO data)
        {           
            return _SendSms.onSelectyear(data);
        }
        [Route("onselectedcourse")]
        public CollegegeneralsmsDTO onselectedcourse([FromBody] CollegegeneralsmsDTO data)
        {           
            return _SendSms.onselectedcourse(data);
        }
        [Route("onselectbranch")]
        public CollegegeneralsmsDTO onselectbranch([FromBody] CollegegeneralsmsDTO data)
        {            
            return _SendSms.onselectbranch(data);
        }

        [Route("onselectsemister")]
        public CollegegeneralsmsDTO onselectsemister([FromBody] CollegegeneralsmsDTO data)
        {           
            return _SendSms.onselectsemister(data);
        }
    }
}
