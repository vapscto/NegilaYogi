using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
   public interface FrontOfficeMonthEndReportInterface
    {
        BirthDayDTO getdata(int id);
        BirthDayDTO getmonthreport(BirthDayDTO data);
    }
}
