
using PreadmissionDTOs.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.HRMS.Interface
{
    public interface HrmsConsolidatedReportInterface
    {

        HRMS_NAAC_DTO getdetails(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_depts(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_desig(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_Employe_ob(HRMS_NAAC_DTO data);
        Task<HRMS_NAAC_DTO> getEmployeReportAsync(HRMS_NAAC_DTO data);
        HRMS_NAAC_DTO get_EmployeALLDATA(HRMS_NAAC_DTO data);
    }
}
