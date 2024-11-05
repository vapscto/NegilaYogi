using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClubManagement.Controllers
{
    [Route("api/[controller]")]
    public class CMS_TrasanctionTypeFacadeController : Controller
    {

        public CMS_TrasanctionTypeInterface _cms;

        public CMS_TrasanctionTypeFacadeController(CMS_TrasanctionTypeInterface cmsdept)
        {
            _cms = cmsdept;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [Route("Getdetails/{id:int}")]
        public CMS_TrasanctionTypeDTO loaddata(int id)
        {
            return _cms.Getdetails(id);
            // return _cms.loaddata(id);
        }
        [Route("GetInitialData/{id:int}")]
        public CMS_TransactionsTypeInstallmentsDTO GetInitialData(int id)
        {
            return _cms.GetInitialData(id);
        }
        //[Route("Getdetails")]
        //public CMS_TrasanctionTypeDTO Getdetails( data)
        //{
        //    return _cms.Getdetails(data);
        //}

        [HttpPost]
        [Route("saveDetails")]
        public CMS_TrasanctionTypeDTO saveDetails([FromBody]CMS_TrasanctionTypeDTO data)
        {
            return _cms.saveDetails(data);
        }

        [Route("editDetails")]
        public CMS_TrasanctionTypeDTO editDetails([FromBody] CMS_TrasanctionTypeDTO id)
        {
            return _cms.editDetails(id);
        }


        [Route("deleteDetails")]
        public CMS_TrasanctionTypeDTO deleteDetails([FromBody] CMS_TrasanctionTypeDTO data)
        {
            return _cms.deleteDetails(data);
        }

        //intallment
       


        [Route("editInsDetails")]
        public CMS_TransactionsTypeInstallmentsDTO editInsDetails([FromBody] CMS_TransactionsTypeInstallmentsDTO id)
        {
            return _cms.editInsDetails(id);
        }


        [HttpPost]
        [Route("saveInsDetails")]
        public CMS_TransactionsTypeInstallmentsDTO saveInsDetails([FromBody]CMS_TransactionsTypeInstallmentsDTO data)
        {
            return _cms.saveInsDetails(data);
        }
       

        [Route("deleteInsDetails")]
        public CMS_TransactionsTypeInstallmentsDTO deleteInsDetails([FromBody] CMS_TransactionsTypeInstallmentsDTO data)
        {
            return _cms.deleteInsDetails(data);
        }

        //transaction tax
        [Route("GetTaxInitialData/{id:int}")]
        public CMS_TransactionsTypeTaxDTO GetTaxInitialData(int id)
        {
            return _cms.GetTaxInitialData(id);
        }

        [Route("deleteTaxDetails")]
        public CMS_TransactionsTypeTaxDTO deleteTaxDetails([FromBody] CMS_TransactionsTypeTaxDTO data)
        {
            return _cms.deleteTaxDetails(data);
        }

        [Route("editTaxDetails")]
        public CMS_TransactionsTypeTaxDTO editTaxDetails([FromBody] CMS_TransactionsTypeTaxDTO id)
        {
            return _cms.editTaxDetails(id);
        }
        [Route("saveTaxDetails")]
        public CMS_TransactionsTypeTaxDTO saveTaxDetails([FromBody]CMS_TransactionsTypeTaxDTO data)
        {
            return _cms.saveTaxDetails(data);
        }

    }
}
