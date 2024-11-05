using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class Clg_YearlyFeeGroupMappingFacade : Controller
    {
        // GET: api/values
        public Clg_YearlyFeeGroupMappingInterface _org;

        public Clg_YearlyFeeGroupMappingFacade(Clg_YearlyFeeGroupMappingInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public CLG_YearlyFeeGroupHeadMapping_DTO Getdet([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails")]
        public CLG_YearlyFeeGroupHeadMapping_DTO Getmasterdetails([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO data)
        {
            return _org.EditMasterscetionDetails(data);
        }


        [Route("getadetailsongroup")]
        public CLG_YearlyFeeGroupHeadMapping_DTO Getgrpdata([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO data)
        {
            return _org.getdataongroup(data);
        }

        [HttpPost]
        public CLG_YearlyFeeGroupHeadMapping_DTO savedata([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO pgmodu)
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
        public CLG_YearlyFeeGroupHeadMapping_DTO Put(int id, [FromBody]CLG_YearlyFeeGroupHeadMapping_DTO value)
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
        public CLG_YearlyFeeGroupHeadMapping_DTO Delete(int id)
        {
            return _org.deleterec(id);
        }

        [Route("selectacademic")]
        public CLG_YearlyFeeGroupHeadMapping_DTO selaca([FromBody] CLG_YearlyFeeGroupHeadMapping_DTO data)
        {
            return _org.selectacade(data);
        }
    }
}
