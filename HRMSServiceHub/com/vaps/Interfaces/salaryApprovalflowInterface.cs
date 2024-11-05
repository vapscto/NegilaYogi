using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
  public interface salaryApprovalflowInterface
    {
        salaryApprovalFlowDTO getBasicData(salaryApprovalFlowDTO dto);

        //FilterEmployeeData

        salaryApprovalFlowDTO FilterEmployeeData(salaryApprovalFlowDTO dto);
        salaryApprovalFlowDTO getEmployeedetailsBySelection(salaryApprovalFlowDTO dto);

        salaryApprovalFlowDTO get_depts(salaryApprovalFlowDTO dto);

        salaryApprovalFlowDTO get_desig(salaryApprovalFlowDTO dto);
    }
}
