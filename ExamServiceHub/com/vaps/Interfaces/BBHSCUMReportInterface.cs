
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface BBHSCUMReportInterface
    {
       Task<BBHSCUMReportDTO> Getdetails(BBHSCUMReportDTO data);
        Task<BBHSCUMReportDTO> savedetails(BBHSCUMReportDTO data);
        BBHSCUMReportDTO getclass(BBHSCUMReportDTO data);
        BBHSCUMReportDTO Getsection(BBHSCUMReportDTO data);
        BBHSCUMReportDTO GetAttendence(BBHSCUMReportDTO data);
        //BBHSCUMReportDTO GetIndividualAttendence(BBHSCUMReportDTO data);

    }
}
