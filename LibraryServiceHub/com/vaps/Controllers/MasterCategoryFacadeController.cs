using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Library;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]

    public class MasterCategoryFacadeController : Controller
    {
        public MasterCategoryInterface _LibInter;
        public MasterCategoryFacadeController(MasterCategoryInterface para)
        {
            _LibInter = para;
        }

        //[HttpPost]
        [Route("Savedata")]
        public MasterCategory_DTO Savedata([FromBody]MasterCategory_DTO data)
        {
            return _LibInter.Savedata(data);
        }
        [Route("deactiveY")]
        public MasterCategory_DTO deactiveY([FromBody]MasterCategory_DTO data)
        {
            return _LibInter.deactiveY(data);
        }

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public MasterCategory_DTO getdetails(int id)
        {
            return _LibInter.getdetails(id);
        }
    }
}
