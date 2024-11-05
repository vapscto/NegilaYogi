using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface SportPointAndRecordReportInterface
    {
        SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO data);


        SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data);
    }
}
