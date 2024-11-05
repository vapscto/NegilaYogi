
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
using Newtonsoft.Json.Linq;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ExamStandardController : Controller
    {

        ExamStandardDelegates ExamStandarddelStr = new ExamStandardDelegates();
      
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public JObject Getdetails(int  id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            var str = JsonConvert.SerializeObject(ExamStandarddelStr.Getdetails(id));
            JObject jobj = JObject.Parse(str);
            return jobj;
            // return ExamStandarddelStr.Getdetails(id);            
        }
        [HttpPost]
        [Route("savedetails")]
        public ExamStandardDTO savedetails([FromBody] ExamStandardDTO data)
        {     
            data.MI_Id= Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           // var str = JsonConvert.SerializeObject(ExamStandarddelStr.savedetails(data));
          //  JObject jobj = JObject.Parse(str);
            return ExamStandarddelStr.savedetails(data);
        }

       
    }

}
