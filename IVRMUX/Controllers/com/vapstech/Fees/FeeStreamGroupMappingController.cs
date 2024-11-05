using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.com.vaps.Fees;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeStreamGroupMappingController : Controller
    {  

        FeeStreamGroupMappingDelegate _driveremp = new FeeStreamGroupMappingDelegate();
        
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("getData/{id:int}")]
        public FeeStreamGroupMappingDTO getData(int data)
        {
            FeeStreamGroupMappingDTO dat = new FeeStreamGroupMappingDTO();
            dat.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.getData(dat);
      
        }

        [Route("Editdetails/{id:int}")]
        public FeeStreamGroupMappingDTO EditDetails(int id)
        {
            HttpContext.Session.SetString("sectionid", id.ToString());
            return _driveremp.EditDetails(id);
        }





        [HttpPost]
        [Route("saveData")]
        public FeeStreamGroupMappingDTO saveData([FromBody] FeeStreamGroupMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _driveremp.saveData(data);
        }
        //[Route("getdetails")]
        //public FeeStreamGroupMappingDTO Edit([FromBody]FeeStreamGroupMappingDTO ids)
        //{
        //    ids.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return deleg.POSTData(ids, "MasterClassCategoryFacade/getdetails/");
        //}
        //[HttpDelete]
        //[Route("deletedetails/{id:int}")]
        //public FeeStreamGroupMappingDTO deletedetails(int id)
        //{
        //    return deleg.DeleteDataById(id, "MasterClassCategoryFacade/deletedetails/");
        //}
        //[Route("deactivate")]
        //public FeeStreamGroupMappingDTO deactivate([FromBody] FeeStreamGroupMappingDTO rel)
        //{
        //    rel.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return _driveremp.saveData(rel);
        //}

        [HttpPost]
        [Route("deactivate")]
        public FeeStreamGroupMappingDTO deactivate([FromBody] FeeStreamGroupMappingDTO id)
        {
            return _driveremp.deactivate(id);
        }


        //[Route("searchByColumn")]
        //public FeeStreamGroupMappingDTO SearchByColumn([FromBody] FeeStreamGroupMappingDTO rel)
        //{
        //    rel.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return deleg.POSTData(rel, "MasterClassCategoryFacade/searchByColumn");
        //}
    }
}
