using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class CoScholasticActivityController : Controller
    {
        CoScholasticActivityDelegates objdelegate = new CoScholasticActivityDelegates();


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [Route("Getdetails")]
        public CoScholasticActivityDTO Getdetails(CoScholasticActivityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return objdelegate.Getdetails(data);
        }


        [Route("savedetails")]
        public CoScholasticActivityDTO savedetail([FromBody] CoScholasticActivityDTO TermAndMap)
        {
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail(TermAndMap);
        }
        [Route("deactivate")]
        public CoScholasticActivityDTO deactivate([FromBody] CoScholasticActivityDTO data)
        {
            return objdelegate.deactivate(data);
        }
        [Route("editdetails/{id:int}")]
        public CoScholasticActivityDTO editdetails(int ID)
        {
            return objdelegate.editdetails(ID);
        }
        //--------------------------------------------------------------
        [Route("savedetails1")]
        public CoScholasticActivityDTO savedetail1([FromBody] CoScholasticActivityDTO TermAndMap)
        {
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail1(TermAndMap);
        }
        [Route("deactivate1")]
        public CoScholasticActivityDTO deactivate1([FromBody] CoScholasticActivityDTO data)
        {
            return objdelegate.deactivate1(data);
        }
        [Route("editdetails1/{id:int}")]
        public CoScholasticActivityDTO editdetails1(int ID)
        {
            return objdelegate.editdetails1(ID);
        }
        [Route("validateordernumber")]
        public CoScholasticActivityDTO validateordernumber([FromBody] CoScholasticActivityDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.validateordernumber(data);
        }
        

        //-- Activites Area Mapping--//
        [Route("savedetail2")]
        public CoScholasticActivityDTO savedetail2([FromBody] CoScholasticActivityDTO TermAndMap)
        {
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.savedetail2(TermAndMap);
        }
        [Route("deactivate2")]
        public CoScholasticActivityDTO deactivate2([FromBody] CoScholasticActivityDTO data)
        {
            return objdelegate.deactivate2(data);
        }
        [Route("editdetails2/{id:int}")]
        public CoScholasticActivityDTO editdetails2(int ID)
        {
            return objdelegate.editdetails2(ID);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]

        [HttpPost]
        [Route("get_exam")]
        public CoScholasticActivityDTO get_exam([FromBody] CoScholasticActivityDTO TermAndMap)
        {
            TermAndMap.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return objdelegate.get_exam(TermAndMap);
        }

        [Route("getexampopup/{id:int}")]
        public CoScholasticActivityDTO getexampopup(int ID)
        {
            return objdelegate.getexampopup(ID);
        }       

       

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }


    }
}
