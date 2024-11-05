using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Canteen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Canteen;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Cantten
{
    [Route("api/[controller]")]
    public class FoodMasterCategoryController : Controller
    {
        FoodMasterCategoryDelegate fmc = new FoodMasterCategoryDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public FoodMasterCategoryDTO loaddata(int id)
        {

            FoodMasterCategoryDTO data = new FoodMasterCategoryDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmc.loaddata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FoodMasterCategoryDTO savedata([FromBody] FoodMasterCategoryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmc.savedata(data);
        }

        [Route("GetEditdata")]
        public FoodMasterCategoryDTO GetEditdata([FromBody] FoodMasterCategoryDTO data)
        {

            return fmc.GetEditdata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FoodMasterCategoryDTO deactvate([FromBody] FoodMasterCategoryDTO data)
        {
            return fmc.deactivate(data);
        }
    }
}
