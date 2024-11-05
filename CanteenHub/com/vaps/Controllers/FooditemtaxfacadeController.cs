using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanteenHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Canteen;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanteenHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FooditemtaxfacadeController : Controller
    {
        public FooditemtaxInterface _fmt;

        public FooditemtaxfacadeController(FooditemtaxInterface fmtdept)
        {
            _fmt = fmtdept;
        }

        [Route("loaddata")]
        public FooditemtaxDTO loaddata([FromBody]FooditemtaxDTO data)
        {
            return _fmt.loaddata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FooditemtaxDTO savedata([FromBody] FooditemtaxDTO data)
        {
            return _fmt.savedata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FooditemtaxDTO deactivate([FromBody] FooditemtaxDTO data)
        {

            return _fmt.deactivate(data);
        }
    }
}
