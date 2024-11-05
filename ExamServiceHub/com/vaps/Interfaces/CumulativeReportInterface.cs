
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface CumulativeReportInterface
    {
        Task<CumulativeReportDTO> savedetails(CumulativeReportDTO data);
        CumulativeReportDTO validateordernumber(CumulativeReportDTO data);
        CumulativeReportDTO deactivate(CumulativeReportDTO data);
        CumulativeReportDTO editdetails(int ID);
        CumulativeReportDTO Getdetails(CumulativeReportDTO data);
        CumulativeReportDTO onchangeyear(CumulativeReportDTO data);
        CumulativeReportDTO onchangeclass(CumulativeReportDTO data);
        CumulativeReportDTO onchangesection(CumulativeReportDTO data);
    }
}
