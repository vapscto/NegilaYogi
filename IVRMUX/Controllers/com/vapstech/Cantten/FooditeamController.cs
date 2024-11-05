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
    //  [Route("api/Fooditeam")]
    public class FooditeamController : Controller
    {
        FooditeamDelegate cms = new FooditeamDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]

        public FooditeamDTO loaddata(int id)
        {
            FooditeamDTO data = new FooditeamDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            return cms.loaddata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FooditeamDTO savedata([FromBody] FooditeamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return cms.savedata(data);
        }

        [Route("GetEditdata")]
        public FooditeamDTO GetEditdata([FromBody] FooditeamDTO data)
        {

            return cms.GetEditdata(data);
        }
        [Route("Getimagedata")]
        public FooditeamDTO Getimagedata([FromBody] FooditeamDTO data)
        {

            return cms.Getimagedata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FooditeamDTO deactvate([FromBody] FooditeamDTO data)
        {
            return cms.deactivate(data);
        }
    }
}

