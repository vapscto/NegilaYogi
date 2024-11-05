using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class PromotionSmsAndEmailDetailsReportDelegate
    {
        CommonDelegate<PromotionSmsAndEmailDetailsReport_DTO, PromotionSmsAndEmailDetailsReport_DTO> comm = new CommonDelegate<PromotionSmsAndEmailDetailsReport_DTO, PromotionSmsAndEmailDetailsReport_DTO>();

        public PromotionSmsAndEmailDetailsReport_DTO getclass(PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return comm.POSTDataExam(data, "PromotionSmsAndEmailDetailsReportFacade/getclass");
        }
        public PromotionSmsAndEmailDetailsReport_DTO getsection(PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return comm.POSTDataExam(data, "PromotionSmsAndEmailDetailsReportFacade/getsection");
        }
        public PromotionSmsAndEmailDetailsReport_DTO loaddata(PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return comm.POSTDataExam(data, "PromotionSmsAndEmailDetailsReportFacade/loaddata");
        }
        public PromotionSmsAndEmailDetailsReport_DTO searchDetails(PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return comm.POSTDataExam(data, "PromotionSmsAndEmailDetailsReportFacade/searchDetails");
        }
        public PromotionSmsAndEmailDetailsReport_DTO SendSmsEmail(PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return comm.POSTDataExam(data, "PromotionSmsAndEmailDetailsReportFacade/SendSmsEmail");
        }

    }
}
