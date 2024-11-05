using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class FeeAmountEntryFacade : Controller
    {
        public FeeAmountEntryInterfaces _org;

        public FeeAmountEntryFacade(FeeAmountEntryInterfaces orga)
        {
            _org = orga;
        }

      
        [Route("getalldetails")]
        public FeeAmountEntryDTO Getdet([FromBody] FeeAmountEntryDTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails")]
        public FeeAmountEntryDTO Getmasterdetails(FeeAmountEntryDTO data)
        {
            return _org.EditMasterscetionDetails(data);
        }

        [HttpPost]
        [Route("paymentdetails")]
        public FeeAmountEntryDTO paymentdetailsfn([FromBody] FeeAmountEntryDTO id)
        {
            return _org.paymentdetailsfnc(id);
        }

        [Route("getgroupmappedheads")]
        public FeeAmountEntryDTO getgroupheaddetails([FromBody] FeeAmountEntryDTO pgmodu)
        {
            return _org.getgroupheaddetails(pgmodu);
        }

       
        public FeeAmountEntryDTO savedata([FromBody] FeeAmountEntryDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            return _org.savedetails(pgmodu);
        }

        [HttpPost("{id}")]
        public FeeAmountEntryDTO Put(int id, [FromBody]FeeAmountEntryDTO value)
        {
            return _org.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

       
        [Route("deletemodpages")]
        public FeeAmountEntryDTO Delete([FromBody] FeeAmountEntryDTO data)
        {
            return _org.deleterec(data);
        }

        [Route("selectacademicyear")]
        public FeeAmountEntryDTO selectacade([FromBody] FeeAmountEntryDTO data)
        {
            return _org.selectacade(data);
        }
        [Route("getalldetailsOnselectiontype")]
        public FeeAmountEntryDTO getalldetailsOnselectiontype([FromBody] FeeAmountEntryDTO data)
        {
            return _org.getalldetailsOnselectiontype(data);
        }
        
    }
}
