using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommonLibrary;
using PreadmissionDTOs;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterClassCategoryController : Controller
    {
        CommonDelegate<MasterClassCategoryDTO, MasterClassCategoryDTO> deleg = new CommonDelegate<MasterClassCategoryDTO, MasterClassCategoryDTO>();

        [HttpGet]
        public MasterClassCategoryDTO getData()
        {
            MasterClassCategoryDTO dto = new MasterClassCategoryDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            // int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //  dto.Id = UserId;

            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));


            return deleg.POSTData(dto, "MasterClassCategoryFacade/getInitialdata/");
        }
        [HttpPost]
        public MasterClassCategoryDTO saveData([FromBody] MasterClassCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.POSTData(data, "MasterClassCategoryFacade");
        }
        [Route("getdetails")]
        public MasterClassCategoryDTO Edit([FromBody]MasterClassCategoryDTO ids)
        {
            ids.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.POSTData(ids, "MasterClassCategoryFacade/getdetails/");
        }
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public MasterClassCategoryDTO deletedetails(int id)
        {
            return deleg.DeleteDataById(id, "MasterClassCategoryFacade/deletedetails/");
        }
        [Route("deactivate")]
        public MasterClassCategoryDTO deactivate([FromBody] MasterClassCategoryDTO rel)
        {
            return deleg.POSTData(rel, "MasterClassCategoryFacade/deactivate");
        }
        [Route("searchByColumn")]
        public MasterClassCategoryDTO SearchByColumn([FromBody] MasterClassCategoryDTO rel)
        {
            rel.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.POSTData(rel, "MasterClassCategoryFacade/searchByColumn");
        }
        [Route("viewrecordspopup")]
        public MasterClassCategoryDTO viewrecordspopup([FromBody] MasterClassCategoryDTO rel)
        {
            rel.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.POSTData(rel, "MasterClassCategoryFacade/viewrecordspopup");
        }
        [Route("deactivesection")]
        public MasterClassCategoryDTO deactivesection([FromBody] MasterClassCategoryDTO rel)
        {
            rel.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return deleg.POSTData(rel, "MasterClassCategoryFacade/deactivesection");
        }
    }
}
