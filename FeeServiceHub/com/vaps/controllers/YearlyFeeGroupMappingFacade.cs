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
    public class YearlyFeeGroupMappingFacade : Controller
    {
        public YearlyFeeGroupMappingInterfaces _org;

        public YearlyFeeGroupMappingFacade(YearlyFeeGroupMappingInterfaces orga)
        {
            _org = orga;
        }
       
       
        [Route("getalldetails")]
        public FeeYearlygroupHeadMappingDTO Getdet([FromBody] FeeYearlygroupHeadMappingDTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails")]
        public FeeYearlygroupHeadMappingDTO Getmasterdetails([FromBody] FeeYearlygroupHeadMappingDTO data)
        {
            return _org.EditMasterscetionDetails(data);
        }

       
        [Route("getadetailsongroup")]
        public FeeYearlygroupHeadMappingDTO Getgrpdata([FromBody] FeeYearlygroupHeadMappingDTO data)
        {
            return _org.getdataongroup(data);
        }

        [HttpPost]
        public FeeYearlygroupHeadMappingDTO savedata([FromBody] FeeYearlygroupHeadMappingDTO pgmodu)
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
        public FeeYearlygroupHeadMappingDTO Put(int id, [FromBody]FeeYearlygroupHeadMappingDTO value)
        {
            return _org.getsearchdata(id, value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [HttpDelete]
        [Route("deletemodpages/{id:int}")]
        public FeeYearlygroupHeadMappingDTO Delete(int id)
        {
            return _org.deleterec(id);
        }

        [Route("selectacademic")]
        public FeeYearlygroupHeadMappingDTO selaca([FromBody] FeeYearlygroupHeadMappingDTO data)
        {
            return _org.selectacade(data);
        }
    }
}
