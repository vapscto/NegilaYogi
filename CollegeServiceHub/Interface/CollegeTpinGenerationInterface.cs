using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;

namespace CollegeServiceHub.Interface
{
    public interface CollegeTpinGenerationInterface
    {
        CollegeTpinGenerationDTO loaddata(CollegeTpinGenerationDTO data);
        CollegeTpinGenerationDTO search(CollegeTpinGenerationDTO data);
        CollegeTpinGenerationDTO generatetpin(CollegeTpinGenerationDTO data);
    }
}
