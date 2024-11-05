using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class PromotionSmsAndEmailDetailsReportFacade : Controller
    {
        public PromotionSmsAndEmailDetailsReportInterface _inter;

        public PromotionSmsAndEmailDetailsReportFacade(PromotionSmsAndEmailDetailsReportInterface inter)
        {
            _inter = inter;
        }
        [Route("getclass")]
        public PromotionSmsAndEmailDetailsReport_DTO getclass([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return _inter.getclass(data);
        }
        [Route("getsection")]
        public PromotionSmsAndEmailDetailsReport_DTO getsection([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return _inter.getsection(data);
        }
        [Route("loaddata")]
        public PromotionSmsAndEmailDetailsReport_DTO loaddata([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("searchDetails")]
        public Task<PromotionSmsAndEmailDetailsReport_DTO> searchDetails([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return _inter.searchDetails(data);
        }
        [Route("SendSmsEmail")]
        public Task<PromotionSmsAndEmailDetailsReport_DTO> SendSmsEmail([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            return _inter.SendSmsEmail(data);
        }
    }
}
