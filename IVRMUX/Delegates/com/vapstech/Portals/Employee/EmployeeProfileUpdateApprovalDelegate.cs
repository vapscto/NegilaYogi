using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Employee;

namespace IVRMUX.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeProfileUpdateApprovalDelegate
    {
        CommonDelegate<EmployeeProfileUpdateApprovalDTO, EmployeeProfileUpdateApprovalDTO> _comm = new CommonDelegate<EmployeeProfileUpdateApprovalDTO, EmployeeProfileUpdateApprovalDTO>();

        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdate(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/loaddataprofileupdate");
        }
        public EmployeeProfileUpdateApprovalDTO Getcastecatgory(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/Getcastecatgory");
        }
        public EmployeeProfileUpdateApprovalDTO Getcaste(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/Getcaste");
        }
        public EmployeeProfileUpdateApprovalDTO SaveData(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/SaveData");
        }

        //Approval Details
        public EmployeeProfileUpdateApprovalDTO loaddataprofileupdateapproval(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/loaddataprofileupdateapproval");
        }
        public EmployeeProfileUpdateApprovalDTO OnChangeOfEmployee(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/OnChangeOfEmployee");
        }
        public EmployeeProfileUpdateApprovalDTO SaveApprovedData(EmployeeProfileUpdateApprovalDTO data)
        {
            return _comm.POSTPORTALData(data, "EmployeeProfileUpdateApprovalFacade/SaveApprovedData");
        }
        
    }
}
