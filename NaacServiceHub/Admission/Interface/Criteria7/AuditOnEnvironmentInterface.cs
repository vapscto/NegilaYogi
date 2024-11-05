using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
    public interface AuditOnEnvironmentInterface
    {
        Task<NAAC_MC_716_AuditOnEnvironment_DTO> loaddata(NAAC_MC_716_AuditOnEnvironment_DTO data);
        NAAC_MC_716_AuditOnEnvironment_DTO savedatatab1(NAAC_MC_716_AuditOnEnvironment_DTO data);
        NAAC_MC_716_AuditOnEnvironment_DTO editTab1(NAAC_MC_716_AuditOnEnvironment_DTO data);
        NAAC_MC_716_AuditOnEnvironment_DTO deactivYTab1(NAAC_MC_716_AuditOnEnvironment_DTO data);
        NAAC_MC_716_AuditOnEnvironment_DTO deleteuploadfile(NAAC_MC_716_AuditOnEnvironment_DTO data);
        NAAC_MC_716_AuditOnEnvironment_DTO getData(NAAC_MC_716_AuditOnEnvironment_DTO data);
    }
}
