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
    public class CMS_TransactionFacade : Controller
    {
        public CMS_TransactionInterface _cms;

        public CMS_TransactionFacade(CMS_TransactionInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_TransactionDTO loaddata(int id)
        {
            return _cms.loaddata(id);
           
        }
        //cmstrandetails
        [Route("loaddatatwo/{id:int}")]
        public CMS_TransactionDetailsDTO loaddatatwo(int id)
        {
            return _cms.loaddatatwo(id);
           
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_TransactionDTO savedata([FromBody]CMS_TransactionDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_TransactionDTO deactive([FromBody]CMS_TransactionDTO data)
        {
            return _cms.deactive(data);
        }

        //edit
        [Route("edit")]
        public CMS_TransactionDTO edit([FromBody]CMS_TransactionDTO data)
        {
            return _cms.edit(data);
        }
        //cmstrandetails

        [Route("savedatatwo")]
        public CMS_TransactionDetailsDTO savedatatwo([FromBody]CMS_TransactionDetailsDTO data)
        {
            return _cms.savedatatwo(data);
        }
        
        [Route("deactivetwo")]
        public CMS_TransactionDetailsDTO deactivetwo([FromBody]CMS_TransactionDetailsDTO data)
        {
            return _cms.deactivetwo(data);
        }

        //edit
        [Route("edittwo")]
        public CMS_TransactionDetailsDTO edittwo([FromBody]CMS_TransactionDetailsDTO data)
        {
            return _cms.edittwo(data);
        }
    }
}
