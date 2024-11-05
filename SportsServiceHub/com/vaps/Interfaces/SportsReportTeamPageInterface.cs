using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportsReportTeamPageInterface
    {
        SportsReportTeamPageDto showdetails(SportsReportTeamPageDto data);
        SportsReportTeamPageDto Getdetails(SportsReportTeamPageDto data);
        SportsReportTeamPageDto get_student(SportsReportTeamPageDto data);
        SportsReportTeamPageDto saveRecord(SportsReportTeamPageDto data);
        SportsReportTeamPageDto deactivate(SportsReportTeamPageDto dto);
        SportsReportTeamPageDto get_modeldata(SportsReportTeamPageDto data);
        SportsReportTeamPageDto EditRecord(SportsReportTeamPageDto dto);
        SportsReportTeamPageDto SaveRecords(SportsReportTeamPageDto data);
        SportsReportTeamPageDto GetEditData(SportsReportTeamPageDto dto);
        SportsReportTeamPageDto deactivated(SportsReportTeamPageDto dto);
    }
}
