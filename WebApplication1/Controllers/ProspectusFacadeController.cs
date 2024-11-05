using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class ProspectusFacadeController : Controller
    {
        public prospectus _pros;

        public ProspectusFacadeController(prospectus prosp)
        {
            _pros = prosp;
        }

    
        [HttpGet]
        [Route("getorganisationcontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public StateDTO Getcountrydata(int id)
        {
            return _pros.enqdrpcountrydata(id);
        }

        [Route("getorganisationstatecontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public CityDTO getcity(int id)
        {
            return _pros.getcity(id);
        }

        [Route("getdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public ProspectusDTO getorgdet(int id)
        {
            // id = 12;
            return _pros.getdetails(id);
        }

        [HttpPost]
        [Route("getEnquiry")]
        //[Route("getenquirycontroller")]
        public Enq getEnqdet([FromBody]searchEnquiryDTO org)
        {
            // id = 12;
            return _pros.getEnqdetails(org);
        }
        // POST api/values
        [HttpPost]
        public async Task<ProspectusDTO> Post([FromBody]ProspectusDTO org)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";

             return await _pros.saveProsdet(org);
            // return det;
        }
        [Route("loaddata")]
        public ProspectusDTO Get([FromBody] ProspectusDTO enqo)
        {
            return _pros.countrydrp(enqo);
        }

        // PUT api/values/5
        [HttpPut]
        public string Put([FromBody]OrganisationDTO org)
        {
            return "success";
        }

        // DELETE api/values/5
        [HttpPost]
        [Route("deletedetails")]
        public ProspectusDTO Deleterec([FromBody]ProspectusDTO org)
        {
            return _pros.deleterec(org);
        }


        // GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        [Route("searchByColumn")]
        public ProspectusDTO searchByColumn([FromBody] ProspectusDTO dto)
        {
            // id = 12;
            return _pros.searchByColumn(dto);
        }

        [Route("getFilePath/{id:int}")]
        public ProspectusDTO getFilePath(int id)
        {
            // id = 12;
            return _pros.getfilePath(id);
        }
        [Route("getpaymentresponse/")]
        public PaymentDetails getpaymentresponse ([FromBody]PaymentDetails response)
        {

            return _pros.payuresponse(response); 
        }
    }
}
