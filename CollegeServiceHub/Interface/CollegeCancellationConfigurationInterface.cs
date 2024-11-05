using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeCancellationConfigurationInterface
    {
        CollegeCancellationConfigurationDTO getdata(CollegeCancellationConfigurationDTO data);
        CollegeCancellationConfigurationDTO saveconfig(CollegeCancellationConfigurationDTO data);
        CollegeCancellationConfigurationDTO editconfig(CollegeCancellationConfigurationDTO data);
        CollegeCancellationConfigurationDTO activedeactive(CollegeCancellationConfigurationDTO data);

    }
}
