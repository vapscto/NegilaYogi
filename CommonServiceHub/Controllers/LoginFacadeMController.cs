using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonServiceHub.Interfaces;
using PreadmissionDTOs.com.vaps.mobile;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Employee;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CommonServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class LoginFacadeMController : Controller
    {
        public LoginMinterface _acd;
        public LoginFacadeMController(LoginMinterface acdm)
        {
            _acd = acdm;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getdetails")]
        public StudentdetDTO getDetails([FromBody] StudentdetDTO.input data)
        {
            return _acd.getdetails(data);
        }

        // [HttpPost]
        [Route("stuattend")]
        public Task<StudentAttendanceDTO> stuattend([FromBody] StudentAttendanceDTO.input data)
        {
            return _acd.getAttend(data);
        }
        [Route("stuYattend")]
        public Task<StudentYAttendanceDTO> stuYattend([FromBody] StudentYAttendanceDTO.input data)
        {
            return _acd.getYAttend(data);
        }
        [Route("stufeedetails")]
        public StudentFeeDetailsDTO stufeedetails([FromBody] StudentFeeDetailsDTO.input data)
        {
            return _acd.stufeedetails(data);
        }
        [Route("StudentFeeTerm")]
        public Task<StudentFeeTermDTO> StudentFeeTerm([FromBody] StudentFeeTermDTO.input data)
        {
            return _acd.StudentFeeTerm(data);
        }
        [Route("CalenderofEvents")]
        public COEDTO CalenderofEvents([FromBody] COEDTO.input data)
        {
            return _acd.CalenderofEvents(data);
        }
        [Route("Examid")]
        public ExamDTO.examid Examid([FromBody] ExamDTO.input data)
        {
            return _acd.Examid(data);
        }
        [Route("Examdetails")]
        public ExamDTO Examdetails([FromBody] ExamDTO.input data)
        {
            return _acd.Examdetails(data);
        }
        [Route("generatehashsequence")]
        public PaymentDetails generatehashsequence([FromBody] OnlinePaymentDTO.input data)
        {
            return _acd.generatehashsequence(data);
        }
        [Route("Timetable")]
        public TTDTO Timetable([FromBody] TTDTO.input data)
        {
            return _acd.Timetable(data);
        }
        //deepak for Employee portal
        [Route("EmployeePortal_SalaryD")]
        public EmpPortalDTO EmployeePortal_SalaryD([FromBody] EmpPortalDTO.Input data)
        {
            return _acd.EmployeePortal_SalaryD(data);
        }
        [Route("EmployeePortal_PunchD")]
        public EmployeePunchDTO EmployeePortal_PunchD([FromBody] EmployeePunchDTO.Input data)
        {
            return _acd.EmployeePortal_PunchD(data);
        }
        [Route("EmployeePortal_StudentAttrndence")]
        public EmployeePortal_StudentAttrndenceDTO EmployeePortal_StudentAttrndence([FromBody] EmployeePortal_StudentAttrndenceDTO.Input data)
        {
            return _acd.EmployeePortal_StudentAttrndence(data);
        }
        [Route("EmployeePortalTimeTableD")]
        public EmployeePortalTimeTableDTO EmployeePortalTimeTableD([FromBody] EmployeePortalTimeTableDTO.Input data)
        {
            return _acd.EmployeePortalTimeTableD(data);
        }
        [Route("EmployeePortalStudentReportCard")]
        public EmployeePortalStudentReportCardDTO EmployeePortalStudentReportCard([FromBody] EmployeePortalStudentReportCardDTO.Input data)
        {
            return _acd.EmployeePortalStudentReportCard(data);
        }
        [Route("EmployeePortalStudentSearchD")]
        public EmployeePortalStudentSearchDTO EmployeePortalStudentSearchD([FromBody] EmployeePortalStudentSearchDTO.Input data)
        {
            return _acd.EmployeePortalStudentSearchD(data);
        }
        [Route("EmployeePortalLeaveD")]
        public EmployeePortalLeaveDTO EmployeePortalLeaveD([FromBody] EmployeePortalLeaveDTO.Input data)
        {
            return _acd.EmployeePortalLeaveD(data);
        }
        [Route("EmployeePortalDetails")]
        public EmployeeDTO EmployeePortalDetails([FromBody] EmployeeDTO.input data)
        {
            return _acd.EmployeePortalDetails(data);
        }

        [Route("EmployeeDetails")]
        public EmployeeloginDTO EmployeeDetails([FromBody] EmployeeloginDTO.input data)
        {
            return _acd.EmployeeDetails(data);
        }

        [Route("EmployeeSalaryDetails")]
        public EmployeeSalaryDTO EmployeeSalaryDetails([FromBody] EmployeeSalaryDTO.input data)
        {
            return _acd.EmployeesalaryDetails(data);
        }
    }
}
