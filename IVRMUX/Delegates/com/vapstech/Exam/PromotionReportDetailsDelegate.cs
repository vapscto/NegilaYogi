using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class PromotionReportDetailsDelegate
    {

        CommonDelegate<PromotionReportDetailsDTO, PromotionReportDetailsDTO> comm = new CommonDelegate<PromotionReportDetailsDTO, PromotionReportDetailsDTO>();

        public PromotionReportDetailsDTO getdata(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data,"PromotionReportDetailsFacade/getdata");
        }
          public PromotionReportDetailsDTO onchangeyear(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/onchangeyear");
        }
        public PromotionReportDetailsDTO onchangeclass(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/onchangeclass");
        }
        public PromotionReportDetailsDTO onchangesection(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/onchangesection");
        }
        public PromotionReportDetailsDTO Report(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/Report");
        }
        public PromotionReportDetailsDTO getpromotioncumulativereport(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/getpromotioncumulativereport");
        }
        public PromotionReportDetailsDTO getpromotioncumulativereport_format2(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/getpromotioncumulativereport_format2");
        }
        public PromotionReportDetailsDTO onpageload(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/onpageload");
        }
        public PromotionReportDetailsDTO PromotionPerformanceReport(PromotionReportDetailsDTO data)
        {
            return comm.POSTDataExam(data, "PromotionReportDetailsFacade/PromotionPerformanceReport");
        }
    }
}
