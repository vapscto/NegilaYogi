using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class PromotionSmsAndEmailDetailsReportController : Controller
    {
        PromotionSmsAndEmailDetailsReportDelegate del = new PromotionSmsAndEmailDetailsReportDelegate();
         
        [Route("getclass")]
        public PromotionSmsAndEmailDetailsReport_DTO getclass([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getclass(data);
        }
        [Route("getsection")]
        public PromotionSmsAndEmailDetailsReport_DTO getsection([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getsection(data);
        }
        [Route("loaddata/{id:int}")]
        public PromotionSmsAndEmailDetailsReport_DTO loaddata(int id)
        {
            PromotionSmsAndEmailDetailsReport_DTO data = new PromotionSmsAndEmailDetailsReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("searchDetails")]
        public PromotionSmsAndEmailDetailsReport_DTO searchDetails([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.searchDetails(data);
        }
        [Route("SendSmsEmail")]
        public PromotionSmsAndEmailDetailsReport_DTO SendSmsEmail([FromBody] PromotionSmsAndEmailDetailsReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.SendSmsEmail(data);
        }
    }
}
