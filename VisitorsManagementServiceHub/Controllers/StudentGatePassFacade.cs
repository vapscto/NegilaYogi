using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class StudentGatePassFacade : Controller
    {
        // GET: api/<controller>

        StudentGatePassInterface _interface;
        public StudentGatePassFacade(StudentGatePassInterface interfaces)
        {
            _interface = interfaces;
        }

        [Route("getdetails")]
        public StudentGatePass_DTO getdetails([FromBody]StudentGatePass_DTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("get_class")]
        public StudentGatePass_DTO get_class([FromBody]StudentGatePass_DTO data)
        {
            return _interface.get_class(data);
        }

        [Route("get_section")]
        public StudentGatePass_DTO get_section([FromBody]StudentGatePass_DTO data)
        {
            return _interface.get_section(data);
        }

        [Route("get_student")]
        public StudentGatePass_DTO get_student([FromBody]StudentGatePass_DTO data)
        {
            return _interface.get_student(data);
        }

        [Route("saverecord")]
        public Task<StudentGatePass_DTO> saverecord([FromBody]StudentGatePass_DTO data)
        {
            return _interface.saverecordAsync(data);
        }

        [Route("editrecord")]
        public StudentGatePass_DTO editrecord([FromBody] StudentGatePass_DTO id)
        {
            return _interface.editrecord(id);
        }

        [Route("deactive")]
        public StudentGatePass_DTO deactive([FromBody]StudentGatePass_DTO data)
        {
            return _interface.deactive(data);
        }

        [Route("checkstudentdata")]
        public StudentGatePass_DTO checkstudentdata([FromBody]StudentGatePass_DTO data)
        {
            return _interface.checkstudentdata(data);
        }

        [Route("get_otpverification")]
        public StudentGatePass_DTO get_otpverification([FromBody]StudentGatePass_DTO data)
        {
            return _interface.get_otpverification(data);
        }

        [Route("resendotp")]
        public Task<StudentGatePass_DTO> resendotp([FromBody]StudentGatePass_DTO data)
        {
            return _interface.resendotp(data);
        }

        [Route("get_otpverification22")]
        public StudentGatePass_DTO get_otpverification22([FromBody]StudentGatePass_DTO data)
        {
            return _interface.get_otpverification22(data);
        }

        [Route("printbutton")]
        public StudentGatePass_DTO printbutton([FromBody]StudentGatePass_DTO data)
        {
            return _interface.printbutton(data);
        }
        [Route("GetStudDetails")]
        public Task<StudentGatePass_DTO> GetStudDetails([FromBody]StudentGatePass_DTO data)
        {
            return _interface.GetStudDetails(data);
        }
        [Route("getotp")]
        public Task<StudentGatePass_DTO> getotp([FromBody]StudentGatePass_DTO data)
        {
            return _interface.getotp(data);
        }

    }
}
