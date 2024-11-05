using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTableServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.TT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTableServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FixingFacadeController : Controller
    {
        public FixingInterface _ttperiod;

        public FixingFacadeController(FixingInterface maspag)
        {
            _ttperiod = maspag;
        }



        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("getdetails")]
        public TTFixingDTO getorgdet([FromBody] TTFixingDTO data)
        {
            return _ttperiod.getdetails(data);
        }
        [HttpPost]
        [Route("savedetail1")]
        public TTFixingDTO Post1([FromBody] TTFixingDTO org)
        {
            return _ttperiod.savedetail1(org);
        }
        [HttpPost]
        [Route("savedetail2")]
        public TTFixingDTO Post2([FromBody] TTFixingDTO org)
        {
            return _ttperiod.savedetail2(org);
        }
        [HttpPost]
        [Route("savedetail3")]
        public TTFixingDTO Post3([FromBody] TTFixingDTO org)
        {
            return _ttperiod.savedetail3(org);
        }
        [HttpPost]
        [Route("savedetail4")]
        public TTFixingDTO Post4([FromBody] TTFixingDTO org)
        {
            return _ttperiod.savedetail4(org);
        }
        [HttpPost]
        [Route("savedetail5")]
        public TTFixingDTO Post5([FromBody] TTFixingDTO org)
        {
            return _ttperiod.savedetail5(org);
        }
        [Route("getpagedetails1/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTFixingDTO getpagedetails1(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit1(id);
        }
        [Route("getpagedetails2/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTFixingDTO getpagedetails2(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit2(id);
        }
        [Route("getpagedetails3/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTFixingDTO getpagedetails3(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit3(id);
        }
        [Route("getpagedetails4/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTFixingDTO getpagedetails4(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit4(id);
        }
        [Route("getpagedetails5/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTFixingDTO getpagedetails5(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit5(id);
        }
        [HttpPost]
        [Route("getcategories")]
        public TTFixingDTO getcategories([FromBody] TTFixingDTO data)
        {
            return _ttperiod.getcategories(data);
        }

        [Route("getalldetailsviewrecords/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords(int id)
        {
            return _ttperiod.getalldetailsviewrecords(id);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        [HttpPost]
        [Route("savedetail")]
        public TTFixingDTO Post([FromBody] TTFixingDTO org)
        {
            return _ttperiod.savedetail(org);
        }

        [HttpPost]
        [Route("getclasses")]
        public TTFixingDTO getclasses([FromBody] TTFixingDTO data)
        {
            return _ttperiod.getclasses(data);
        }
        [HttpPost]
        [Route("getperiods")]
        public TTFixingDTO getperiods([FromBody] TTFixingDTO data)
        {
            return _ttperiod.getperiods(data);
        }
        [HttpPost]
        [Route("getstaff")]
        public TTFixingDTO getstaff([FromBody] TTFixingDTO data)
        {
            return _ttperiod.getstaff(data);
        }
        [HttpPost]
        [Route("getsubjects")]
        public TTFixingDTO getsubjects([FromBody] TTFixingDTO data)
        {
            return _ttperiod.getsubjects(data);
        }
        [HttpPost]
        [Route("get_cls_sec_subs")]
        public TTFixingDTO get_cls_sec_subs([FromBody] TTFixingDTO data)
        {
            return _ttperiod.get_cls_sec_subs(data);
        }
        [HttpPost]
        [Route("get_cls_sec_staffs")]
        public TTFixingDTO get_cls_sec_staffs([FromBody] TTFixingDTO data)
        {
            return _ttperiod.get_cls_sec_staffs(data);
        }
        [Route("getalldetailsviewrecords2/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords2(int id)
        {
            return _ttperiod.getalldetailsviewrecords2(id);
        }
        [Route("getalldetailsviewrecords3/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords3(int id)
        {
            return _ttperiod.getalldetailsviewrecords3(id);
        }
        [Route("getalldetailsviewrecords4/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords4(int id)
        {
            return _ttperiod.getalldetailsviewrecords4(id);
        }
        [Route("getalldetailsviewrecords5/{id:int}")]
        public TTFixingDTO getalldetailsviewrecords5(int id)
        {
            return _ttperiod.getalldetailsviewrecords5(id);
        }

        [HttpPost]
        [Route("deactivate1")]
        public TTFixingDTO deactivate1([FromBody] TTFixingDTO org)
        {
            return _ttperiod.deactivate1(org);
        }
        [HttpPost]
        [Route("deactivate2")]
        public TTFixingDTO deactivate2([FromBody] TTFixingDTO org)
        {
            return _ttperiod.deactivate2(org);
        }
        [HttpPost]
        [Route("deactivate3")]
        public TTFixingDTO deactivate3([FromBody] TTFixingDTO org)
        {
            return _ttperiod.deactivate3(org);
        }
        [HttpPost]
        [Route("deactivate4")]
        public TTFixingDTO deactivate4([FromBody] TTFixingDTO org)
        {
            return _ttperiod.deactivate4(org);
        }
        [HttpPost]
        [Route("deactivate5")]
        public TTFixingDTO deactivate5([FromBody] TTFixingDTO org)
        {
            return _ttperiod.deactivate5(org);
        }
        [HttpPost]
        [Route("deactivate")]
        public TTFixingDTO deactivate([FromBody] TTFixingDTO org)
        {
            return _ttperiod.deactivate(org);
        }

        [Route("getpagedetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public TTFixingDTO getpagedetails(int id)
        {
            // id = 12;
            return _ttperiod.getpageedit(id);
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public TTFixingDTO Deleterec(int id)
        {
            return _ttperiod.deleterec(id);
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
    }
}
