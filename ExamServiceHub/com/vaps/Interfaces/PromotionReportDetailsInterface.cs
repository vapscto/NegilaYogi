using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface PromotionReportDetailsInterface
    {
        PromotionReportDetailsDTO getdata(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO onchangeyear(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO onchangeclass(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO onchangesection(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO Report(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO getpromotioncumulativereport(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO getpromotioncumulativereport_format2(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO onpageload(PromotionReportDetailsDTO data);
        PromotionReportDetailsDTO PromotionPerformanceReport(PromotionReportDetailsDTO data);
    }
}
