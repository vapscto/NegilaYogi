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
    public class FooditemtaxController : Controller
    {
        FooditemtaxDelegate fmt = new FooditemtaxDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]

        public FooditemtaxDTO loaddata(int id)
        {
            FooditemtaxDTO data = new FooditemtaxDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmt.loaddata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FooditemtaxDTO savedata([FromBody] FooditemtaxDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmt.savedata(data);
        }

        [HttpPost]
        [Route("deactivate")]
        public FooditemtaxDTO deactvate([FromBody] FooditemtaxDTO data)
        {
            return fmt.deactivate(data);
        }
    }
}
