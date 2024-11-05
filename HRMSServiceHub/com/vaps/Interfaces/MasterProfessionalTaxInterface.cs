using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterProfessionalTaxInterface
    {
        HR_Master_ProfessionalTaxDTO getBasicData(HR_Master_ProfessionalTaxDTO dto);
        HR_Master_ProfessionalTaxDTO SaveUpdate(HR_Master_ProfessionalTaxDTO dto);
        HR_Master_ProfessionalTaxDTO editData(int id);
        HR_Master_ProfessionalTaxDTO deactivate(HR_Master_ProfessionalTaxDTO dto);

    }
}
