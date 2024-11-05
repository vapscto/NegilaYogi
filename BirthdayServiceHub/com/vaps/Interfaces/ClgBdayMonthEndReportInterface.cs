using PreadmissionDTOs.com.vaps.College.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayServiceHub.com.vaps.Interfaces
{
    public interface ClgBdayMonthEndReportInterface
    {
        ClgBirthDayDTO getloaddata(ClgBirthDayDTO data);
        Task<ClgBirthDayDTO> getmonthreport(ClgBirthDayDTO data);
    }
}
