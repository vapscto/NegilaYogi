
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class PromotionCalculationController : Controller
    {
        PromotionCalculationDelegates crStr = new PromotionCalculationDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public PromotionCalculationDTO Getdetails(PromotionCalculationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return crStr.Getdetails(data);
        }

        [HttpPost]
        [Route("get_cls_sections")]
        public PromotionCalculationDTO get_cls_sections([FromBody] PromotionCalculationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return crStr.get_cls_sections(categorypage);
        }

        [Route("Calculation")]
        public PromotionCalculationDTO Calculation([FromBody] PromotionCalculationDTO categorypage)
        {
            categorypage.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            categorypage.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return crStr.Calculation(categorypage);
        }
        [Route("get_classes")]
        public PromotionCalculationDTO onselectAcdYear([FromBody] PromotionCalculationDTO dto)
        {
            dto.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.get_classes(dto);
        }

        [Route("onchangesection")]
        public PromotionCalculationDTO onchangesection([FromBody] PromotionCalculationDTO dto)
        {
            dto.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.onchangesection(dto);
        }

        [Route("publishtostudentportal")]
        public PromotionCalculationDTO publishtostudentportal([FromBody] PromotionCalculationDTO dto)
        {
            dto.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return crStr.publishtostudentportal(dto);
        }
    }
}
