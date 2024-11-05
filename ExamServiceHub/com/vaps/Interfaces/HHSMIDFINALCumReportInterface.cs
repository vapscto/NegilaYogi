
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface HHSMIDFINALCumReportInterface
    {
        Task<HHSMIDFINALCumReportDTO> savedetails(HHSMIDFINALCumReportDTO data);
        Task<HHSMIDFINALCumReportDTO> savedetailsnew(HHSMIDFINALCumReportDTO data);        
        HHSMIDFINALCumReportDTO validateordernumber(HHSMIDFINALCumReportDTO data);
        HHSMIDFINALCumReportDTO Getdetails(HHSMIDFINALCumReportDTO data);
        HHSMIDFINALCumReportDTO cumulativereport(HHSMIDFINALCumReportDTO data);
        HHSMIDFINALCumReportDTO ExamSubExamCumulativeReport(HHSMIDFINALCumReportDTO data);
    }
}
