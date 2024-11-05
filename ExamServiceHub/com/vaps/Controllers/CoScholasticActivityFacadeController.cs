using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using ExamServiceHub.com.vaps.Interfaces;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CoScholasticActivityFacadeController : Controller
    {
        public CoScholasticActivityInterface _ttcategory;

        public CoScholasticActivityFacadeController(CoScholasticActivityInterface maspag)
        {
            _ttcategory = maspag;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("Getdetails")]
        public CoScholasticActivityDTO Getdetails([FromBody] CoScholasticActivityDTO data)//int IVRMM_Id
        {
            return _ttcategory.Getdetails(data);
        }


        [HttpPost]
        [Route("savedetail")]
        public CoScholasticActivityDTO Post([FromBody] CoScholasticActivityDTO org)
        {
            return _ttcategory.savedetail(org);
        }

        [Route("editdetails/{id:int}")]
        public CoScholasticActivityDTO editdetails(int ID)
        {
            return _ttcategory.editdetails(ID);
        }

        [Route("deactivate")]
        public CoScholasticActivityDTO deactivate([FromBody] CoScholasticActivityDTO data)
        {
            return _ttcategory.deactivate(data);
        }
        //----------------------------------------------------------------------------------------

        [HttpPost]
        [Route("savedetail1")]
        public CoScholasticActivityDTO Post1([FromBody] CoScholasticActivityDTO org)
        {
            return _ttcategory.savedetail1(org);
        }

        [Route("editdetails1/{id:int}")]
        public CoScholasticActivityDTO editdetails1(int ID)
        {
            return _ttcategory.editdetails1(ID);
        }

        [Route("deactivate1")]
        public CoScholasticActivityDTO deactivate1([FromBody] CoScholasticActivityDTO data)
        {
            return _ttcategory.deactivate1(data);
        }
        [Route("validateordernumber")]
        public CoScholasticActivityDTO validateordernumber([FromBody] CoScholasticActivityDTO data)
        {
            return _ttcategory.validateordernumber(data);
        }


        //------- Activites Area Mapping
        [Route("savedetail2")]
        public CoScholasticActivityDTO savedetail2([FromBody] CoScholasticActivityDTO org)
        {
            return _ttcategory.savedetail2(org);
        }

        [Route("editdetails2/{id:int}")]
        public CoScholasticActivityDTO editdetails2(int ID)
        {
            return _ttcategory.editdetails2(ID);
        }

        [Route("deactivate2")]
        public CoScholasticActivityDTO deactivate2([FromBody] CoScholasticActivityDTO data)
        {
            return _ttcategory.deactivate2(data);
        }





        //POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

       
       
        [HttpPost]
        [Route("get_exam")]
        public CoScholasticActivityDTO get_exam([FromBody] CoScholasticActivityDTO org)
        {
            return _ttcategory.get_exam(org);
        }
        [Route("getexampopup/{id:int}")]
        public CoScholasticActivityDTO getexampopup(int ID)
        {
            return _ttcategory.getexampopup(ID);
        }


      

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
