using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface HSEligibilityCerficateInterface
    {
        HSEligibilityCerficateDTO Getdetails(HSEligibilityCerficateDTO data);
        HSEligibilityCerficateDTO get_class(HSEligibilityCerficateDTO data);
        HSEligibilityCerficateDTO get_section(HSEligibilityCerficateDTO data);
        HSEligibilityCerficateDTO get_student(HSEligibilityCerficateDTO data);
        HSEligibilityCerficateDTO get_age(HSEligibilityCerficateDTO data);
        HSEligibilityCerficateDTO get_certificate(HSEligibilityCerficateDTO data);
        HSEligibilityCerficateDTO get_PUcertificate(HSEligibilityCerficateDTO data);

    }
}
