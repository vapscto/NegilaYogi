using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportTopperListReportInterface
    {
        Task<SportStudentParticipationReportDTO> showdetails(SportStudentParticipationReportDTO data);
        SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data);
        SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO data);
        SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO data);
        SportStudentParticipationReportDTO get_student(SportStudentParticipationReportDTO data);
        Task<SportStudentParticipationReportDTO> Kiosk_Sports_Details(SportStudentParticipationReportDTO data);
    }
}
