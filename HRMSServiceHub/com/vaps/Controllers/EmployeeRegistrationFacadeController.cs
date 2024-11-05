using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeRegistrationFacadeController : Controller
    {
        public EmployeeRegistrationInterface _ads;

        public EmployeeRegistrationFacadeController(EmployeeRegistrationInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public MasterEmployeeDTO getinitialdata([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("getdepartment")]
        public MasterEmployeeDTO getdepartment([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getdepartment(dto);
        }

        [Route("getdesignation")]
        public MasterEmployeeDTO getdesignation([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getdesignation(dto);
        }

        // POST api/values
        [HttpPost]
        public MasterEmployeeDTO Post([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.SaveUpdate(dto);
        }

        [Route("getRecordById/{id:int}")]
        public MasterEmployeeDTO getcatgrydet(int id)
        {
            return _ads.editData(id);
        }
        [Route("deactivateRecordById")]
        public MasterEmployeeDTO deactivateRecordById([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.deactivate(dto);
        }

        //state dropdown

        [Route("getstateDropdownByCountryId/{id:int}")]

        public MasterEmployeeDTO getstateDropdownByCountryId(int id)
        {
            return _ads.getstateDropdown(id);
        }

        //validate data

        [Route("validatedata")]
        public MasterEmployeeDTO validatedata([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.validateData(dto);
        }


        [Route("SalaryCalculation")]
        public MasterEmployeeDTO SalaryCalculation([FromBody]HR_Master_Employee_IncrementDetailsDTO dto)
        {
            return _ads.SalaryCalculation(dto);
        }

        //setemporder
        [Route("employeesetorder")]
        public MasterEmployeeDTO employeesetorder([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.employeesetorder(dto);
        }




        //setemporder
        [Route("DeleteQualificationRecord")]
        public Master_Employee_QulaificationDTO DeleteQualificationRecord([FromBody]Master_Employee_QulaificationDTO dto)
        {
            return _ads.DeleteQualificationRecord(dto);
        }

        //setemporder
        [Route("DeleteExperienceRecord")]
        public Master_Employee_ExperienceDTO DeleteExperienceRecord([FromBody]Master_Employee_ExperienceDTO dto)
        {
            return _ads.DeleteExperienceRecord(dto);
        }
        //setemporder
        [Route("DeleteDocumentRecord")]
        public Master_Employee_DocumentsDTO DeleteDocumentRecord([FromBody]Master_Employee_DocumentsDTO dto)
        {
            return _ads.DeleteDocumentRecord(dto);
        }


        //Added by sudeep

        //duplicate mobile number
        [Route("duplicate_mob")]
        public MasterEmployeeDTO duplicate_mob([FromBody]Mobile_Number_DTO dto)
        {
            return _ads.Dupl_mob(dto);
        }
        [Route("chk_dup_email")]
        public MasterEmployeeDTO chk_dup_email([FromBody]Email_Id_DTO dto)
        {
            return _ads.Dupl_email(dto);
        }

        [Route("duplicate_bankAccountNo")]
        public MasterEmployeeDTO duplicate_bankAccountNo([FromBody]HR_Master_Employee_BankDTO dto)
        {
            return _ads.duplicate_bankAccountNo(dto);
        }



        [Route("delete_mob")]
        public MasterEmployeeDTO del_mob([FromBody]Mobile_Number_DTO dto)
        {
            return _ads.del_mob(dto);
        }
        [Route("del_email")]
        public MasterEmployeeDTO del_email([FromBody]Email_Id_DTO dto)
        {
            return _ads.del_email(dto);
        }

        [Route("delete_bankAccountNo")]
        public MasterEmployeeDTO delete_bankAccountNo([FromBody]HR_Master_Employee_BankDTO dto)
        {
            return _ads.delete_bankAccountNo(dto);
        }



        //setemporder
        [Route("getEmployeeSalaryDetails")]
        public MasterEmployeeDTO getEmployeeSalaryDetails([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getEmployeeSalaryDetails(dto);
        }


        //setemporder
        [Route("getEmployeeSalaryDetailsByHead")]
        public MasterEmployeeDTO getEmployeeSalaryDetailsByHead([FromBody]HR_Employee_EarningsDeductionsDTO dto)
        {
            return _ads.getEmployeeSalaryDetailsByHead(dto);
        }

        [Route("getcaste")]
        public MasterEmployeeDTO getcaste([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getcaste(dto);
        }

        [Route("getcastecatgory")]
        public MasterEmployeeDTO getcastecatgory([FromBody]MasterEmployeeDTO dto)
        {
            return _ads.getcastecatgory(dto);
        }
    }
}
