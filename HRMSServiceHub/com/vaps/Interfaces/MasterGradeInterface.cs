using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Interfaces
{
    public interface MasterGradeInterface
    {
        HR_Master_GradeDTO getBasicData(HR_Master_GradeDTO dto);
        HR_Master_GradeDTO SaveUpdate(HR_Master_GradeDTO dto);

        HR_Master_GradeDTO changeorderData(HR_Master_GradeDTO dto);
        HR_Master_GradeDTO editData(int id);

        HR_Master_GradeDTO deactivate(HR_Master_GradeDTO dto);
    }
}
