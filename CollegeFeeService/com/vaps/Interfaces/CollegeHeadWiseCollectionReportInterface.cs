using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeHeadWiseCollectionReportInterface
    {
        CollegeConcessionDTO getdetails(CollegeConcessionDTO dt);


        CollegeConcessionDTO get_courses(CollegeConcessionDTO data);
        CollegeConcessionDTO get_branches(CollegeConcessionDTO data);
        CollegeConcessionDTO get_semisters(CollegeConcessionDTO data); 
        CollegeConcessionDTO getgroupmappedheads(CollegeConcessionDTO data);
        Task<CollegeConcessionDTO> radiobtndata(CollegeConcessionDTO data);
    }
}
