
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface HHSAllReportInterface
    {
        Task<HHSAllReportDTO> Getdetails(HHSAllReportDTO data);
        Task<HHSAllReportDTO> savedetails(HHSAllReportDTO data);
        HHSAllReportDTO yearchange(HHSAllReportDTO data);
        HHSAllReportDTO classchange(HHSAllReportDTO data);
        HHSAllReportDTO sectionchange(HHSAllReportDTO data);
        Task<HHSAllReportDTO> getbbkvreport(HHSAllReportDTO data);       
    }
}
