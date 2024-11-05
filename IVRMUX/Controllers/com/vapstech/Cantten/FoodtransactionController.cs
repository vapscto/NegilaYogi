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
    public class FoodtransactionController : Controller
    {
        FoodtransactionDelegate fmt = new FoodtransactionDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]

        public FoodtransactionDTO loaddata(int id)
        {
            FoodtransactionDTO data = new FoodtransactionDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmt.loaddata(data);
        }
        [HttpPost]
        [Route("FoodItem")]
        public FoodtransactionDTO FoodItem([FromBody] FoodtransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmt.FoodItem(data);
        }
        [HttpPost]
        [Route("FoodItemtax")]
        public FoodtransactionDTO FoodItemtax([FromBody] FoodtransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmt.FoodItemtax(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FoodtransactionDTO savedata([FromBody] FoodtransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return fmt.savedata(data);
        }

    
        [HttpPost]
        [Route("deactivate")]
        public FoodtransactionDTO deactvate([FromBody] FoodtransactionDTO data)
        {
            return fmt.deactivate(data);
        }
    }
}
