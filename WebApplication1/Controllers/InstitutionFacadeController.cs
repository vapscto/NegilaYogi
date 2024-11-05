using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class InstitutionFacadeController : Controller
    {
        public Institutioninterface _enq;
        public InstitutionFacadeController(Institutioninterface Instit)
        {
            _enq = Instit;
        }
        //// GET: api/values
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

        //// GET: api/values
        //[HttpGet]
        //public InstitutionDTO Get(InstitutionDTO enqo)
        //{
        //    return _enq.countrydrp(enqo);
        //}

        [Route("getinstitutioncontroller/{id:int}")]
        public StateDTO Getcountrydata(int id)
        {
            return _enq.getStatedataByCountryID(id);
        }

        [Route("getinstitutionstatecontroller/{id:int}")]
        //[Route("getenquirycontroller")]
        public CityDTO getcity(int id)
        {
            return _enq.getcity(id);
        }

        [Route("getdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public InstitutionDTO getorgdet(int id)
        {            
            return _enq.getdetails(id);
        }

        // POST api/values
        [HttpPost]
        public InstitutionDTO Post([FromBody] InstitutionDTO Institution)
        {
            return _enq.saveInstitute(Institution);
        }

        [Route("getdetailsById/{id:int}")]
        public InstitutionDTO getInstutedetById(int id)
        {
            return _enq.getdetails(id);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public InstitutionDTO Deleterec(int id)
        {
            return _enq.deleterec(id);
        }

        [Route("getAllDetails")]
        public async Task<InstitutionDTO> Getdata([FromBody] InstitutionDTO InstitutionDTO)
        {
            return await _enq.OnPageloadData(InstitutionDTO);
        }

        [Route("DuplicateDataFind")]
        public InstitutionDTO DuplicateDataFind([FromBody] InstitutionDTO Institution)
        {
            return _enq.DuplicateData(Institution);
        }

        [Route("SaveSubscriptionValidity")]
        public Master_Institution_SubscriptionValidityDTO SaveSubscriptionValidity([FromBody] Master_Institution_SubscriptionValidityDTO sb)
        {
            return _enq.SaveSubscriptionValidity(sb);
        }

        [HttpDelete]
        [Route("deleteSubscriptiondetails/{id:int}")]
        public Master_Institution_SubscriptionValidityDTO DeleteSubscriptiondetails(int id)
        {
            return _enq.deleteSubscriptionrec(id);
        }       

        [Route("getInstitutionSearchedDetails")]
        public async Task<InstitutionDTO> getInstitutionSearchedDetails([FromBody] SortingPagingInfoDTO id)
        {
            return await _enq.Institutionsearchdata(id);
        }

        [Route("getSubscriptionSearchedDetails")]
        public async Task<Master_Institution_SubscriptionValidityDTO> getSubscriptionSearchedDetails([FromBody] SortingPagingInfoDTO id)
        {
            return await _enq.Subscriptionsearchdata(id);
        }

        [Route("OnClickSaveAutoMapping")]
        public InstitutionDTO OnClickSaveAutoMapping([FromBody] InstitutionDTO Ins)
        {
            return _enq.OnClickSaveAutoMapping(Ins);
        }
    }
}