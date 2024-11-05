using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
  public  interface HolidayReportInterface
    {
        MasterHolidayDTO getdata(int id);
        MasterHolidayDTO ReportList(MasterHolidayDTO stu1);
    }
}
