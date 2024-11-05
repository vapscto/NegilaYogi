using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

namespace AlumniHub.Com.Facade
{
   
    [Route("api/[controller]")]
    public class Alumni_GalleryFacadeController : Controller
    {
        public Alumni_Gallery_Interface _ads;
        public Alumni_GalleryFacadeController(Alumni_Gallery_Interface ads)
        {
            _ads = ads;
        }
        [HttpPost]
        [Route("getloaddata")]
        public Alumni_GalleryDTO getloaddata([FromBody]Alumni_GalleryDTO data)
        {
            return _ads.getloaddata(data);
        }

        [Route("savedata")]
        public Alumni_GalleryDTO savedata([FromBody]Alumni_GalleryDTO data)
        {
            return _ads.savedata(data);
        }
        [Route("getcovermodel")]
        public Alumni_GalleryDTO getcovermodel([FromBody]Alumni_GalleryDTO data)
        {
            return _ads.getcovermodel(data);
        }
        [Route("savecover")]
        public Alumni_GalleryDTO savecover([FromBody]Alumni_GalleryDTO data)
        {
            return _ads.savecover(data);
        }
        [Route("deactive")]
        public Alumni_GalleryDTO deactive([FromBody]Alumni_GalleryDTO data)
        {
            return _ads.deactive(data);
        }
    }
}