using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.College.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClgBirthdayServiceHub.com.vaps.Interfaces
{
    public interface BdayMonthEndReportInterface
    {
        ClgBirthDayDTO getloaddata(ClgBirthDayDTO data);
        Task<ClgBirthDayDTO> getmonthreport(ClgBirthDayDTO data);
       
        
    }
}
