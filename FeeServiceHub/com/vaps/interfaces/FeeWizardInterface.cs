using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeWizardInterface
    {
        FeeWizardDTO getdetails(FeeWizardDTO data);
        FeeWizardDTO SaveYearlyGroupData(FeeWizardDTO org);
        FeeWizardDTO changacademicyear(FeeWizardDTO org);
        FeeWizardDTO deactivateY(FeeWizardDTO id);

        FeeWizardDTO savedetailsFGH(FeeWizardDTO pgmod);
        
        FeeWizardDTO deleterec(int id);
        FeeWizardDTO savedetailYCC(FeeWizardDTO pgmod);
        FeeWizardDTO deleterecY(int id);
        FeeWizardDTO savedetailFMA(FeeWizardDTO pgmod);
        FeeWizardDTO savedetailFMAG(FeeWizardDTO pgmod);
        FeeWizardDTO deleterecfma(FeeWizardDTO data);
        FeeWizardDTO deletedta(FeeWizardDTO data);
    }
}
