using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class OrganisationFacadeController : Controller
    {
        public Organisationinterface _org;

        public OrganisationFacadeController(Organisationinterface orga)
        {
            _org = orga;
        }

        //// GET: api/values
        [HttpPost]
        [Route("getalldetails")]
        public Task<OrganisationDTO> Get([FromBody] OrganisationDTO enqo)
        {
            return _org.countrydrp(enqo);
        }

        [Route("getorganisationcontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public StateDTO Getcountrydata(int id)
        {
            return _org.enqdrpcountrydata(id);
        }

        //[Route("getfiltereddetails/{id:int}")]
        //public OrganisationDTO getfiltereddetails(int filtype)
        //{
        //    return _org.getfilterdet(filtype);
        //}

        [Route("getorganisationstatecontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public CountryDTO getcity(int id)
        {
            return _org.getcity(id);
        }

        [Route("getdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public OrganisationDTO getorgdet(int id)
        {
           // id = 12;
            return _org.getdetails(id);
        }

        [Route("getcurrencydetails/{id:int}")]
        public OrganisationDTO getcurrencydet(int id)
        {
            return _org.getcurrency(id);
        }

        // POST api/values
        [HttpPost]
        public OrganisationDTO Post([FromBody]OrganisationDTO org)
        {
            // OrganisationDTO det = new OrganisationDTO();
            // det.IVRMMCT_Id = "45";
            return  _org.saveorgdet(org);
           // return det;
        }

        // PUT api/values/5
        [HttpPut]
        public string Put([FromBody]OrganisationDTO org)
        {
            return "success";
        }

        [HttpPost("{id}")]
        public OrganisationDTO Put(int id, [FromBody]OrganisationDTO value)
        {
            return _org.getfilterdet(id, value);
        }

        [Route("getOrganisationSearchedDetails")]
        public async Task<OrganisationDTO> getOrganisationSearchedDetails([FromBody] SortingPagingInfoDTO Ins)
        {
            return await _org.getorgSearchedDetails(Ins);
        }


        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public OrganisationDTO Deleterec(int id)
        { 
            return _org.deleterec(id);
        }
    }
}
