
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface SNSPROGRESSCARDReportInterface
    {
        Task<SNSPROGRESSCARDReportDTO> Getdetails(SNSPROGRESSCARDReportDTO data);
        Task<SNSPROGRESSCARDReportDTO> savedetails(SNSPROGRESSCARDReportDTO data);
        SNSPROGRESSCARDReportDTO yearchange(SNSPROGRESSCARDReportDTO data);
        SNSPROGRESSCARDReportDTO classchange(SNSPROGRESSCARDReportDTO data);
        SNSPROGRESSCARDReportDTO sectionchange(SNSPROGRESSCARDReportDTO data);
    }
}
