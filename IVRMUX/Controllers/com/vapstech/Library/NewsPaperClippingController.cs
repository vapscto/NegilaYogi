using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Library
{
    [Route("api/[controller]")]
    public class NewsPaperClippingController : Controller
    {
        // GET: api/<controller>

        NewsPaperClippingDelegate _notic = new NewsPaperClippingDelegate();
       
        [Route("savedetail")]
        public ImageClipping_DTO savedetail([FromBody]ImageClipping_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public ImageClipping_DTO Getdetails(int id)
        {
            ImageClipping_DTO obj = new ImageClipping_DTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _notic.Getdetails(obj);
        }
        [Route("deactivate")]
        public ImageClipping_DTO deactivate([FromBody]ImageClipping_DTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return _notic.deactivate(data);
        }
        [Route("editdetails")]
        public ImageClipping_DTO editdetails([FromBody]ImageClipping_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _notic.editdetails(data);
        }




    }
}
