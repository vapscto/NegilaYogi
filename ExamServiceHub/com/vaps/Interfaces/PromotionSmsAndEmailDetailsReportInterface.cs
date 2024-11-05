using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface PromotionSmsAndEmailDetailsReportInterface
    {
        PromotionSmsAndEmailDetailsReport_DTO getclass(PromotionSmsAndEmailDetailsReport_DTO data);
        PromotionSmsAndEmailDetailsReport_DTO getsection(PromotionSmsAndEmailDetailsReport_DTO data);
        PromotionSmsAndEmailDetailsReport_DTO loaddata(PromotionSmsAndEmailDetailsReport_DTO data);
        Task<PromotionSmsAndEmailDetailsReport_DTO> searchDetails(PromotionSmsAndEmailDetailsReport_DTO data);
        Task<PromotionSmsAndEmailDetailsReport_DTO> SendSmsEmail(PromotionSmsAndEmailDetailsReport_DTO data);
    }
}
