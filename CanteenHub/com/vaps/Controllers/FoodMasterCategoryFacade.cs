using CanteenHub.com.vaps.Interfaces;
using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FoodMasterCategoryFacade : Controller
    {
        public FoodMasterCategoryInterface _fmc;

        public FoodMasterCategoryFacade(FoodMasterCategoryInterface fmcdept)
        {
            _fmc = fmcdept;
        }

        [Route("loaddata")]
        public FoodMasterCategoryDTO loaddata([FromBody]FoodMasterCategoryDTO data)
        {
            return _fmc.loaddata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FoodMasterCategoryDTO savedata([FromBody] FoodMasterCategoryDTO data)
        {
            return _fmc.savedata(data);
        }
        [Route("GetEditdata")]
        public FoodMasterCategoryDTO GetEditdata([FromBody] FoodMasterCategoryDTO data)
        {

            return _fmc.GetEditdata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FoodMasterCategoryDTO deactivate([FromBody] FoodMasterCategoryDTO data)
        {

            return _fmc.deactivate(data);
        }
    }
}
