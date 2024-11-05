using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportFitnessReportInterface
    {
        SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO data);

        SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO data);
        SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO data);
        SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data);
    }
}
