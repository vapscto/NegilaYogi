using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.ClubManagement;
using IVRMUX.Delegates.com.vapstech.ClubManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.ClubManagement
{
    [Route("api/[controller]")]
    public class CMS_TransactionTypeController : Controller
    {
        CMS_TransactionTypeDelegate gate = new CMS_TransactionTypeDelegate();
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

        [HttpGet]
        [Route("Getdetails/{id:int}")]

        public CMS_TrasanctionTypeDTO Getdetails(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                  
            return gate.Getdetails(id);
         
        }

        [HttpPost]
        [Route("saveDetails")]

        public CMS_TrasanctionTypeDTO saveDetails([FromBody] CMS_TrasanctionTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.saveDetails(data);
        }
        [Route("editDetails")]

        public CMS_TrasanctionTypeDTO editDetails([FromBody] CMS_TrasanctionTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.editDetails(data);
        }


        //[Route("editDetails/{id:int}")]
        //public CMS_TrasanctionTypeDTO editDetails(int id)
        //{
        //    return gate.editdata(id);
        //}

        [Route("deleteDetails")]
        public CMS_TrasanctionTypeDTO deleteDetails([FromBody] CMS_TrasanctionTypeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return gate.deleteDetails(data);
        }
        //inatallment
    
      
        [HttpGet]
        [Route("GetInitialData/{id:int}")]
        public CMS_TransactionsTypeInstallmentsDTO GetInitialData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return gate.GetInitialData(id);

        }


        [Route("editInsDetails")]
        public CMS_TransactionsTypeInstallmentsDTO editInsDetails([FromBody] CMS_TransactionsTypeInstallmentsDTO data)
        {
           // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.editInsDetails(data);
        }
        [HttpPost]
        [Route("saveInsDetails")]

        public CMS_TransactionsTypeInstallmentsDTO saveInsDetails([FromBody] CMS_TransactionsTypeInstallmentsDTO data)
        {
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.saveInsDetails(data);
        }

        [Route("deleteInsDetails")]
        public CMS_TransactionsTypeInstallmentsDTO deleteInsDetails([FromBody] CMS_TransactionsTypeInstallmentsDTO data)
        {
           
            return gate.deleteInsDetails(data);
        }
        //transaction tax

        [HttpGet]
        [Route("GetTaxInitialData/{id:int}")]
        public CMS_TransactionsTypeTaxDTO GetTaxInitialData(int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return gate.GetTaxInitialData(id);

        }

        [Route("deleteTaxDetails")]
        public CMS_TransactionsTypeTaxDTO deleteTaxDetails([FromBody] CMS_TransactionsTypeTaxDTO data)
        {

            return gate.deleteTaxDetails(data);
        }

        [Route("editTaxDetails")]
        public CMS_TransactionsTypeTaxDTO editTaxDetails([FromBody] CMS_TransactionsTypeTaxDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.editTaxDetails(data);
        }

        [HttpPost]
        [Route("saveTaxDetails")]

        public CMS_TransactionsTypeTaxDTO saveTaxDetails([FromBody] CMS_TransactionsTypeTaxDTO data)
        {
            data.user_id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return gate.saveTaxDetails(data);
        }
    }

}
