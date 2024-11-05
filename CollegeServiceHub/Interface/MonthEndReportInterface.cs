using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface MonthEndReportInterface
    {
        MonthEndReportDTO getdata(MonthEndReportDTO data);
        Task<MonthEndReportDTO> getreport(MonthEndReportDTO data);

        MonthEndReportDTO getyear(MonthEndReportDTO data);
        Task<MonthEndReportDTO> Studdetails(MonthEndReportDTO data);

        MonthEndReportDTO getbranch(MonthEndReportDTO data);

        MonthEndReportDTO getsemester(MonthEndReportDTO data);
    }
}
