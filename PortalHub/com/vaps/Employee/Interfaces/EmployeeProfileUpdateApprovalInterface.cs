using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeProfileUpdateApprovalInterface
    {
        EmployeeProfileUpdateApprovalDTO loaddataprofileupdate(EmployeeProfileUpdateApprovalDTO data);
        EmployeeProfileUpdateApprovalDTO Getcastecatgory(EmployeeProfileUpdateApprovalDTO data);
        EmployeeProfileUpdateApprovalDTO Getcaste(EmployeeProfileUpdateApprovalDTO data);
        EmployeeProfileUpdateApprovalDTO SaveData(EmployeeProfileUpdateApprovalDTO data);

        //Approval Details
        EmployeeProfileUpdateApprovalDTO loaddataprofileupdateapproval(EmployeeProfileUpdateApprovalDTO data);
        EmployeeProfileUpdateApprovalDTO OnChangeOfEmployee(EmployeeProfileUpdateApprovalDTO data);
        EmployeeProfileUpdateApprovalDTO SaveApprovedData(EmployeeProfileUpdateApprovalDTO data);
    }
}