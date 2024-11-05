using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class NewsPaperClippingFacade : Controller
    {
        // GET: api/<controller>

        public NewsPaperClippingInterface _work;
        public NewsPaperClippingFacade(NewsPaperClippingInterface work)
        {
            _work = work;
        }
        // GET: api/values
        [Route("savedetail")]
        public ImageClipping_DTO savedetail([FromBody]ImageClipping_DTO data)
        {
            return _work.savedetail(data);
        }
        [Route("Getdetails")]
        public ImageClipping_DTO Getdetails([FromBody]ImageClipping_DTO data)
        {
            return _work.Getdetails(data);
        }
        [Route("deactivate")]
        public ImageClipping_DTO deactivate([FromBody]ImageClipping_DTO data)
        {
            return _work.deactivate(data);
        }
        [Route("editdetails")]
        public ImageClipping_DTO editdetails([FromBody]ImageClipping_DTO data)
        {
            return _work.editdetails(data);
        }


    }
}
