using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeDefaultersReportInterface
    {
        CollegeConcessionDTO getdetails(CollegeConcessionDTO dt);
        CollegeConcessionDTO get_courses(CollegeConcessionDTO data);
        CollegeConcessionDTO get_branches(CollegeConcessionDTO data);
        CollegeConcessionDTO get_semisters(CollegeConcessionDTO data);
        Task<CollegeConcessionDTO> radiobtndata(CollegeConcessionDTO data);
        Task<FeetransactionSMS> sendsms(FeetransactionSMS data);
        FeetransactionSMS sendemail(FeetransactionSMS data);
        CollegeConcessionDTO duedateExcededRemainder(CollegeConcessionDTO data);
        CollegeConcessionDTO DuedateRemainder(CollegeConcessionDTO data);
        Task<CollegeConcessionDTO> duedatesms(CollegeConcessionDTO data);
      
    }
}
