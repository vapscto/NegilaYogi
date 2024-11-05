
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
    public class exammastercategoryFacadeController : Controller
    {
        public exammastercategoryInterface _ttcategory;

        public exammastercategoryFacadeController(exammastercategoryInterface maspag)
        {
            _ttcategory = maspag;
        }



        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public exammastercategoryDTO getorgdet(int id)
        {
            return _ttcategory.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail1")]
        public exammastercategoryDTO Post1([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.savedetail1(org);
        }
        [HttpPost]
        [Route("savedetail2")]
        public exammastercategoryDTO Post2([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.savedetail2(org);
        }
        [Route("getalldetailsviewrecords")]
        public exammastercategoryDTO getalldetailsviewrecords([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.getalldetailsviewrecords(org);
        }
        [Route("deactivate_sec")]
        public exammastercategoryDTO deactivate_sec([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.deactivate_sec(org);
        }
        [HttpPost]
        [Route("geteventdetails")]
        public exammastercategoryDTO geteventdetails([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.geteventdetails(org);
        }

        [HttpPost]
        [Route("deactivate1")]
        public exammastercategoryDTO deactivate1([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.deactivate1(org);
        }
        [HttpPost]
        [Route("deactivate2")]
        public exammastercategoryDTO deactivate2([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.deactivate2(org);
        }
        [Route("getalldetailsviewrecords1/{id:int}")]
        //[Route("getenquirycontroller")]
        public exammastercategoryDTO getalldetailsviewrecords1(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords1(id);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        //[Route("getenquirycontroller")]
        public exammastercategoryDTO getalldetailsviewrecords2(int id)
        {
            // id = 12;
            return _ttcategory.getalldetailsviewrecords2(id);
        }
        [Route("getpagedetails1/{id:int}")]
        //[Route("getenquirycontroller")]
        public exammastercategoryDTO getpagedetails1(int id)
        {
            // id = 12;
            return _ttcategory.getpageedit1(id);
        }
        [Route("getpagedetails2")]
        //[Route("getenquirycontroller")]
        public exammastercategoryDTO getpagedetails2([FromBody] exammastercategoryDTO org)
        {
            // id = 12;
            return _ttcategory.getpageedit2(org);
        }
        [Route("get_cate_class")]
        //[Route("getenquirycontroller")]
        public exammastercategoryDTO get_cate_class([FromBody] exammastercategoryDTO org)
        {
            // id = 12;
            return _ttcategory.get_cate_class(org);
        }
        [Route("get_cls_sections")]
        public exammastercategoryDTO get_cls_sections([FromBody] exammastercategoryDTO org)
        {
            // id = 12;
            return _ttcategory.get_cls_sections(org);
        }

        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public exammastercategoryDTO Deleterec(int id)
        {
            return _ttcategory.deleterec(id);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("Save_ReportCard_Format")]
        public exammastercategoryDTO Save_ReportCard_Format([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.Save_ReportCard_Format(org);
        }

        [Route("deactive_format")]
        public exammastercategoryDTO deactive_format([FromBody] exammastercategoryDTO org)
        {
            return _ttcategory.deactive_format(org);
        }
    }
}
