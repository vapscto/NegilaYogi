using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface EmployeeRegistrationInterface
    {
        MasterEmployeeDTO getBasicData(MasterEmployeeDTO dto);
        MasterEmployeeDTO SaveUpdate(MasterEmployeeDTO dto);
        MasterEmployeeDTO editData(int id);
        MasterEmployeeDTO deactivate(MasterEmployeeDTO dto);
        MasterEmployeeDTO getstateDropdown(int countryId);
        MasterEmployeeDTO validateData(MasterEmployeeDTO dto);
        MasterEmployeeDTO SalaryCalculation(HR_Master_Employee_IncrementDetailsDTO dto);
        //setemporder
        MasterEmployeeDTO employeesetorder(MasterEmployeeDTO dto);
        Master_Employee_QulaificationDTO DeleteQualificationRecord(Master_Employee_QulaificationDTO dto);
        Master_Employee_ExperienceDTO DeleteExperienceRecord(Master_Employee_ExperienceDTO dto);
        Master_Employee_DocumentsDTO DeleteDocumentRecord(Master_Employee_DocumentsDTO dto);
        //Added by sudeep
        MasterEmployeeDTO Dupl_mob(Mobile_Number_DTO dto);
        MasterEmployeeDTO Dupl_email(Email_Id_DTO dto);
        MasterEmployeeDTO duplicate_bankAccountNo(HR_Master_Employee_BankDTO dto);
        MasterEmployeeDTO del_mob(Mobile_Number_DTO dto);
        MasterEmployeeDTO del_email(Email_Id_DTO dto);
        MasterEmployeeDTO delete_bankAccountNo(HR_Master_Employee_BankDTO dto);
        MasterEmployeeDTO getEmployeeSalaryDetails(MasterEmployeeDTO dto);
        MasterEmployeeDTO getEmployeeSalaryDetailsByHead(HR_Employee_EarningsDeductionsDTO dto);
        MasterEmployeeDTO getcaste(MasterEmployeeDTO dto);
        MasterEmployeeDTO getcastecatgory(MasterEmployeeDTO dto);
        MasterEmployeeDTO getdepartment(MasterEmployeeDTO dto);
        MasterEmployeeDTO getdesignation(MasterEmployeeDTO dto);
    }
}
