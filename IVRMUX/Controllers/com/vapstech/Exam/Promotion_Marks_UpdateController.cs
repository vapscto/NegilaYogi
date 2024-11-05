using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class Promotion_Marks_UpdateController : Controller
    {
        Promotion_Marks_UpdateDelegates del_fr = new Promotion_Marks_UpdateDelegates();

        [HttpGet]
        [Route("Getdetails")]
        public Promotion_Marks_UpdateDTO Getdetails(Promotion_Marks_UpdateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.Getdetails(data);
        }
        [HttpPost]
        [Route("get_categories")]
        public Promotion_Marks_UpdateDTO get_categories([FromBody] Promotion_Marks_UpdateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_categories(data);
        }
        [Route("get_classes")]
        public Promotion_Marks_UpdateDTO get_classes([FromBody] Promotion_Marks_UpdateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_classes(data);
        }       
        [Route("get_sections")]
        public Promotion_Marks_UpdateDTO get_sections([FromBody] Promotion_Marks_UpdateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_sections(data);
        }
        [Route("get_subjects")]
        public Promotion_Marks_UpdateDTO get_subjects([FromBody] Promotion_Marks_UpdateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_subjects(data);
        }
        [Route("get_prommarks")]
        public Promotion_Marks_UpdateDTO get_prommarks([FromBody] Promotion_Marks_UpdateDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del_fr.get_prommarks(data);
        }
    }
}
