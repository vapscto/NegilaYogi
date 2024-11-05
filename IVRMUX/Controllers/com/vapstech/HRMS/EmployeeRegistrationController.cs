using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeRegistrationController : Controller
    {
        EmployeeRegistrationDelegate del = new EmployeeRegistrationDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterEmployeeDTO getalldetails(int id)
        {
            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        [Route("getdepartment")]
        public MasterEmployeeDTO getdepartment([FromBody]MasterEmployeeDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdepartment(dto);
        }

        [Route("getdesignation")]
        public MasterEmployeeDTO getdesignation([FromBody]MasterEmployeeDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getdesignation(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public MasterEmployeeDTO Post([FromBody]MasterEmployeeDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public MasterEmployeeDTO editRecord(int id)
        {
            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public MasterEmployeeDTO ActiveDeactiveRecord(int id)
        {
            MasterEmployeeDTO dto = new MasterEmployeeDTO();
            dto.HRME_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.deleterec(dto);
        }


        [Route("getstateDropdownByCountryId/{id:int}")]
        public MasterEmployeeDTO getstateDropdownByCountryId(int id)
        {
            return del.getstateDropdownByCountryId(id);
        }

        //validate data
        [Route("validateData")]
        public MasterEmployeeDTO validateData([FromBody]MasterEmployeeDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.validatedata(dto);
        }



        //Salary calculation

        //validate data
        [Route("SalaryCalculation")]
        public MasterEmployeeDTO SalaryCalculation([FromBody]HR_Master_Employee_IncrementDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.SalaryCalculation(dto);


        }

        //Onchange
        [Route("validateordernumber")]
        public MasterEmployeeDTO setorder([FromBody] MasterEmployeeDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("session_MI_id"));
            return del.employeesetorder(dto);
        }



        //DeleteQualificationRecord data
        [Route("DeleteQualificationRecord")]
        public Master_Employee_QulaificationDTO DeleteQualificationRecord([FromBody]Master_Employee_QulaificationDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.DeleteQualificationRecord(dto);
        }


        //DeleteExperienceRecord data
        [Route("DeleteExperienceRecord")]
        public Master_Employee_ExperienceDTO DeleteExperienceRecord([FromBody]Master_Employee_ExperienceDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.DeleteExperienceRecord(dto);
        }

        //DeleteDocumentRecord data
        [Route("DeleteDocumentRecord")]
        public Master_Employee_DocumentsDTO DeleteDocumentRecord([FromBody]Master_Employee_DocumentsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.DeleteDocumentRecord(dto);
        }
        //getEmployeeSalaryDetails
        [Route("getEmployeeSalaryDetails")]
        public MasterEmployeeDTO getEmployeeSalaryDetails([FromBody] MasterEmployeeDTO dto)
        {
            return del.getEmployeeSalaryDetails(dto);
        }


        //getEmployeeSalaryDetails
        [Route("getEmployeeSalaryDetailsByHead")]
        public MasterEmployeeDTO getEmployeeSalaryDetailsByHead([FromBody] HR_Employee_EarningsDeductionsDTO dto)
        {
            return del.getEmployeeSalaryDetailsByHead(dto);
        }



        //duplicate check

        [Route("chk_dup_mob")]
        public MasterEmployeeDTO chk_dup_mob([FromBody] Mobile_Number_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.duplicate_mob(dto);
        }

        [Route("chk_dup_email")]
        public MasterEmployeeDTO chk_dup_email([FromBody] Email_Id_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.chk_dup_email(dto);
        }
        [Route("duplicate_bankAccountNo")]
        public MasterEmployeeDTO duplicate_bankAccountNo([FromBody] HR_Master_Employee_BankDTO dto)
        {
            return del.duplicate_bankAccountNo(dto);
        }


        //del_mob


        [Route("del_mob")]
        public MasterEmployeeDTO del_mob([FromBody] Mobile_Number_DTO dto)
        {
            return del.del_mob(dto);
        }
        [Route("del_email")]
        public MasterEmployeeDTO del_email([FromBody] Email_Id_DTO dto)
        {
            return del.del_email(dto);
        }
        [Route("del_bankAccount")]
        public MasterEmployeeDTO del_bankAccount([FromBody] HR_Master_Employee_BankDTO dto)
        {
            return del.del_bankAccount(dto);
        }

        [Route("getcaste")]
        public MasterEmployeeDTO getcaste([FromBody] MasterEmployeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcaste(data);
        }

        [Route("getcastecatgory")]
        public MasterEmployeeDTO getcastecatgory([FromBody] MasterEmployeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcastecatgory(data);
        }
    }
}
