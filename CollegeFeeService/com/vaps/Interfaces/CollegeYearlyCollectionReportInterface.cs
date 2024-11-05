using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeYearlyCollectionReportInterface
    {
        CollegeConcessionDTO getdetails(CollegeConcessionDTO dt);


        CollegeConcessionDTO get_courses(CollegeConcessionDTO data);
        CollegeConcessionDTO get_branches(CollegeConcessionDTO data);
        CollegeConcessionDTO get_semisters(CollegeConcessionDTO data);
        CollegeConcessionDTO get_semisters_new(CollegeConcessionDTO data);
        CollegeConcessionDTO getgroupmappedheads(CollegeConcessionDTO feedto);

        CollegeConcessionDTO getgroupheadsid(CollegeConcessionDTO feedtohead);
        Task<CollegeConcessionDTO> Getreportdetails(CollegeConcessionDTO feedtoget);
        CollegeConcessionDTO getdata(CollegeConcessionDTO feedtohead);
        //headwisedetails
        Task<CollegeConcessionDTO> headwisedetails(CollegeConcessionDTO feedtoget);

    }
}
