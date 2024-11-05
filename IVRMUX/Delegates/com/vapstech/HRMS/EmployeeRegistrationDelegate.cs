using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class EmployeeRegistrationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterEmployeeDTO, MasterEmployeeDTO> COMMM = new CommonDelegate<MasterEmployeeDTO, MasterEmployeeDTO>();

        CommonDelegate<MasterEmployeeDTO, HR_Master_Employee_IncrementDetailsDTO> COMMM1 = new CommonDelegate<MasterEmployeeDTO, HR_Master_Employee_IncrementDetailsDTO>();



        CommonDelegate<Master_Employee_QulaificationDTO, Master_Employee_QulaificationDTO> COMMMQulaification = new CommonDelegate<Master_Employee_QulaificationDTO, Master_Employee_QulaificationDTO>();
        CommonDelegate<Master_Employee_ExperienceDTO, Master_Employee_ExperienceDTO> COMMMExperience = new CommonDelegate<Master_Employee_ExperienceDTO, Master_Employee_ExperienceDTO>();
        CommonDelegate<Master_Employee_DocumentsDTO, Master_Employee_DocumentsDTO> COMMMDocuments = new CommonDelegate<Master_Employee_DocumentsDTO, Master_Employee_DocumentsDTO>();

        CommonDelegate<MasterEmployeeDTO, HR_Employee_EarningsDeductionsDTO> COMMM2 = new CommonDelegate<MasterEmployeeDTO, HR_Employee_EarningsDeductionsDTO>();


        CommonDelegate<MasterEmployeeDTO, Mobile_Number_DTO> COMMMMobile = new CommonDelegate<MasterEmployeeDTO, Mobile_Number_DTO>();
        CommonDelegate<MasterEmployeeDTO, Email_Id_DTO> COMMMEmail = new CommonDelegate<MasterEmployeeDTO, Email_Id_DTO>();
        CommonDelegate<MasterEmployeeDTO, HR_Master_Employee_BankDTO> COMMMAccountNo = new CommonDelegate<MasterEmployeeDTO, HR_Master_Employee_BankDTO>();


        public MasterEmployeeDTO onloadgetdetails(MasterEmployeeDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeRegistrationFacade/onloadgetdetails");
        }

        public MasterEmployeeDTO getdepartment(MasterEmployeeDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeRegistrationFacade/getdepartment");
        }

        public MasterEmployeeDTO getdesignation(MasterEmployeeDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "EmployeeRegistrationFacade/getdesignation");
        }

        public MasterEmployeeDTO savedetails(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/");
        }
        public MasterEmployeeDTO getRecorddetailsById(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "EmployeeRegistrationFacade/getRecordById/");
        }
        public MasterEmployeeDTO deleterec(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/deactivateRecordById/");
        }

        //getstateDropdownByCountryId
        public MasterEmployeeDTO getstateDropdownByCountryId(int id)
        {
            return COMMM.GetDataByIdHRMS(id, "EmployeeRegistrationFacade/getstateDropdownByCountryId/");
        }

        //validate data DeleteQualificationRecord
        public MasterEmployeeDTO validatedata(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/validatedata/");
        }



        //salary calculation
        public MasterEmployeeDTO SalaryCalculation(HR_Master_Employee_IncrementDetailsDTO maspage)
        {
            return COMMM1.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/SalaryCalculation/");
        }


        public MasterEmployeeDTO employeesetorder(MasterEmployeeDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/employeesetorder/");
        }


        //DeleteQualificationRecord  
        public Master_Employee_QulaificationDTO DeleteQualificationRecord(Master_Employee_QulaificationDTO maspage)
        {
            return COMMMQulaification.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/DeleteQualificationRecord/");
        }

        // DeleteExperienceRecord
        public Master_Employee_ExperienceDTO DeleteExperienceRecord(Master_Employee_ExperienceDTO maspage)
        {
            return COMMMExperience.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/DeleteExperienceRecord/");
        }

        // DeleteDocumentRecord
        public Master_Employee_DocumentsDTO DeleteDocumentRecord(Master_Employee_DocumentsDTO maspage)
        {
            return COMMMDocuments.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/DeleteDocumentRecord/");
        }


        //

        public MasterEmployeeDTO getEmployeeSalaryDetails(MasterEmployeeDTO maspage)
        {
            return COMMM1.POSTDataHRMS(maspage, "EmployeeRegistrationFacade/getEmployeeSalaryDetails/");
        }

        //getEmployeeSalaryDetailsByHead

        public MasterEmployeeDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO maspage)
        {
            return COMMM2.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/getEmployeeSalaryDetailsByHead/");
        }



        //duplicate mobile number
        public MasterEmployeeDTO duplicate_mob(Mobile_Number_DTO maspage)
        {
            return COMMMMobile.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/duplicate_mob/");
        }

        //duplicate Email Id
        public MasterEmployeeDTO chk_dup_email(Email_Id_DTO maspage)
        {
            return COMMMEmail.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/chk_dup_email/");
        }

        //duplicate bankAccount number
        public MasterEmployeeDTO duplicate_bankAccountNo(HR_Master_Employee_BankDTO maspage)
        {
            return COMMMAccountNo.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/duplicate_bankAccountNo/");
        }



        //delete mobile number
        public MasterEmployeeDTO del_mob(Mobile_Number_DTO maspage)
        {
            return COMMMMobile.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/delete_mob/");
        }


        //delete Email Id
        public MasterEmployeeDTO del_email(Email_Id_DTO maspage)
        {
            return COMMMEmail.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/del_email/");
        }
        //delete bankAccount number
        public MasterEmployeeDTO del_bankAccount(HR_Master_Employee_BankDTO maspage)
        {
            return COMMMAccountNo.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/delete_bankAccountNo/");
        }
        public MasterEmployeeDTO getcaste(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/getcaste/");
        }

        public MasterEmployeeDTO getcastecatgory(MasterEmployeeDTO maspage)
        {
            return COMMM.POSTDataaHRMS(maspage, "EmployeeRegistrationFacade/getcastecatgory/");
        }
    }
}
