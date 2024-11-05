using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeAccountsPositionReportInterface
    {
        CollegeConcessionDTO getdata(CollegeConcessionDTO data);
        CollegeConcessionDTO getgroupByCG(CollegeConcessionDTO data);
        CollegeConcessionDTO getReport(CollegeConcessionDTO obj);
    }
}
