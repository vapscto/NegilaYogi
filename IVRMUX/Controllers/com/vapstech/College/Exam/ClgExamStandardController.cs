using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam
{
    [Route("api/[controller]")]
    public class ClgExamStandardController : Controller
    {

        ClgExamStandardDelegates ExamStandarddelStr = new ClgExamStandardDelegates();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("Getdetails")]
        public JObject Getdetails(int id)
        {
            ExamStandardDTO obj = new ExamStandardDTO();
            obj.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            var str = JsonConvert.SerializeObject(ExamStandarddelStr.Getdetails(obj));
            JObject jobj = JObject.Parse(str);
            return jobj;
        }
        [HttpPost]
        [Route("savedetails")]
        public ExamStandardDTO savedetails([FromBody] ExamStandardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ExamStandarddelStr.savedetails(data);
        }


    }

}
