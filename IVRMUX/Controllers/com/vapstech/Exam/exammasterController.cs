
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
    public class exammasterController : Controller
    {
        exammasterDelegates exammasterdelStr = new exammasterDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public exammasterDTO Getdetails(exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.Getdetails(data);            
        }

        [Route("editdetails/{id:int}")]
        public exammasterDTO editdetails(int ID)
        {
            return exammasterdelStr.editdetails(ID);
        }
        
        [Route("validateordernumber")]
        public exammasterDTO validateordernumber([FromBody] exammasterDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.validateordernumber(data);
        }

        [Route("savedetails")]
        public exammasterDTO savedetails([FromBody] exammasterDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.savedetails(data);
        }

        [Route("deactivate")]
        public exammasterDTO deactivate([FromBody] exammasterDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.deactivate(data);         
        }

        // Master Exam Paper Type
        [Route("BindData_PaperType/{id:int}")]
        public exammasterDTO BindData_PaperType(int ID)
        {
            exammasterDTO data = new exammasterDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.BindData_PaperType(data);
        }

        [Route("Saveddata_PT")]
        public exammasterDTO Saveddata_PT([FromBody] exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.Saveddata_PT(data);
        }

        [Route("Editdata_PT")]
        public exammasterDTO Editdata_PT([FromBody] exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.Editdata_PT(data);
        }

        [Route("DeactivateActivateMasterExam_PT")]
        public exammasterDTO DeactivateActivateMasterExam_PT([FromBody] exammasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return exammasterdelStr.DeactivateActivateMasterExam_PT(data);
        }
    }
}