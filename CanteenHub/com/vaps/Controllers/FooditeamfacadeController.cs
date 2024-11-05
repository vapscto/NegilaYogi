using CanteenHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Controllers
{
    
    [Route("api/[controller]")]
    public class FooditeamfacadeController : Controller
    {
        public FooditeamInterface _cms;

        public FooditeamfacadeController(FooditeamInterface cmsdept)
        {
            _cms = cmsdept;
        }
           
        [Route("loaddata")]
        public FooditeamDTO loaddata([FromBody]FooditeamDTO data)
        {
            return _cms.loaddata(data);
        }

        [HttpPost]
        [Route("savedata")]
        public FooditeamDTO savedata([FromBody] FooditeamDTO data)
        {
            return _cms.savedata(data);
        }

        [Route("GetEditdata")]
        public FooditeamDTO GetEditdata([FromBody] FooditeamDTO data)
        {

            return _cms.GetEditdata(data);
        }
        [Route("Getimagedata")]
        public FooditeamDTO Getimagedata([FromBody] FooditeamDTO data)
        {

            return _cms.Getimagedata(data);
        }
        [HttpPost]
        [Route("deactivate")]
        public FooditeamDTO deactivate([FromBody] FooditeamDTO data)
        {

            return _cms.deactivate(data);
        }


        [HttpPost]
        [Route("Createpin")]
        public FooditeamDTO Createpin([FromBody] FooditeamDTO data)
        {
            return _cms.Createpin(data);
        }
        [HttpPost]
        [Route("changepassword")]
        public FooditeamDTO changepassword([FromBody] FooditeamDTO data)
        {
            return _cms.changepassword(data);
        }

        [HttpPost]
        [Route("Forgotpin")]
        public FooditeamDTO Forgotpin([FromBody] FooditeamDTO data)
        {
            return _cms.Forgotpin(data);
        }
       
    }
}

