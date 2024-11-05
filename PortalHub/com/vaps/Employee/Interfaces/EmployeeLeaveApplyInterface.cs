using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface EmployeeLeaveApplyInterface
    {
        EmployeeDashboardDTO getonlineLeave(EmployeeDashboardDTO data);
        EmployeeDashboardDTO saveonlineLeave(EmployeeDashboardDTO data);

        //======================== TC Class teacher approval
        Adm_TC_Approval_DTO getdata_CTA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO SaveEdit_CTA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO details_CTA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO deactivate_CTA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO getstudetails_CTA(Adm_TC_Approval_DTO dto);


        //======================== TC library approval
        Adm_TC_Approval_DTO getdata_LIB(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO SaveEdit_LIB(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO details_LIB(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO deactivate_LIB(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO getstudetails_LIB(Adm_TC_Approval_DTO dto);

        //======================== TC FEE approval
        Adm_TC_Approval_DTO getdata_FEE(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO SaveEdit_FEE(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO details_FEE(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO deactivate_FEE(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO getstudetails_FEE(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO feeheaddetails_FEE(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO feenot_approval_FEE(Adm_TC_Approval_DTO dto);

        //======================== FDA Class teacher approval
        Adm_TC_Approval_DTO getdata_PDA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO SaveEdit_PDA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO details_PDA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO deactivate_PDA(Adm_TC_Approval_DTO dto);
        Adm_TC_Approval_DTO getstudetails_PDA(Adm_TC_Approval_DTO dto);

    }
}
